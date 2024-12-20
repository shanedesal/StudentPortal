﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models.Entities;
using StudentPortal.Models;
using Microsoft.AspNetCore.Authorization;

namespace StudentPortal.Controllers
{
    [Authorize]
    public class SubjectsController : Controller
    {
        private readonly ApplicationDBContext dBContext;
        public SubjectsController(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public IActionResult Add(string searchTerm = null)
        {
            var subjects = string.IsNullOrEmpty(searchTerm)
                ? dBContext.Subjects.ToList()
                : dBContext.Subjects
                          .Where(s => s.SubjectCode.Contains(searchTerm) || s.Description.Contains(searchTerm))
                          .ToList();

            var viewModel = new AddSubjectViewModel
            {
                SubjectsTable = subjects,
                PreRequisiteCode = TempData["SelectedPreRequisite"]?.ToString()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectViewModel viewModel)
        {
            try
            {
                // Validate PreRequisiteCode if provided
                if (!string.IsNullOrEmpty(viewModel.PreRequisiteCode))
                {
                    var prerequisiteExists = await dBContext.Subjects
                        .AnyAsync(s => s.SubjectCode == viewModel.PreRequisiteCode);

                    if (!prerequisiteExists)
                    {
                        ModelState.AddModelError("PreRequisiteCode", "Invalid prerequisite subject code");
                        viewModel.SubjectsTable = await dBContext.Subjects.ToListAsync();
                        return View(viewModel);
                    }
                }

                // Validate if subject code already exists
                var subjectExists = await dBContext.Subjects
                    .AnyAsync(s => s.SubjectCode == viewModel.SubjectCode);

                if (subjectExists)
                {
                    ModelState.AddModelError("SubjectCode", "Subject code already exists");
                    viewModel.SubjectsTable = await dBContext.Subjects.ToListAsync();
                    return View(viewModel);
                }

                var subject = new Subject
                {
                    SubjectCode = viewModel.SubjectCode,
                    Description = viewModel.Description,
                    Units = viewModel.Units,
                    Offering = viewModel.Offering,
                    Category = viewModel.Category,
                    CourseCode = viewModel.CourseCode,
                    CurriculumYear = viewModel.CurriculumYear,
                    PreRequisiteCode = string.IsNullOrEmpty(viewModel.PreRequisiteCode) ? null : viewModel.PreRequisiteCode
                };

                await dBContext.Subjects.AddAsync(subject);
                await dBContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Subject added successfully";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving the subject. Please try again.");
                viewModel.SubjectsTable = await dBContext.Subjects.ToListAsync();
                return View(viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> List(string sortOrder, string currentFilter, string searchString)
        {
            // Save the sort order in ViewData
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SubjectCodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "subject_code_desc" : "";
            ViewData["DescriptionSortParm"] = sortOrder == "description" ? "description_desc" : "description";

            // If search string is changed, reset page
            if (searchString != null)
            {
                currentFilter = searchString;
            }

            // Save the filter in ViewData
            ViewData["CurrentFilter"] = currentFilter;

            // Get the base query
            var subjects = dBContext.Subjects
                .Include(s => s.PreRequisite)
                .AsQueryable();

            // Apply search filter if provided
            if (!String.IsNullOrEmpty(currentFilter))
            {
                subjects = subjects.Where(s =>
                    s.SubjectCode.Contains(currentFilter) ||
                    s.Description.Contains(currentFilter) ||
                    s.CourseCode.Contains(currentFilter) ||
                    s.Category.Contains(currentFilter) ||
                    s.Offering.Contains(currentFilter)
                );
            }

            // Apply sorting
            subjects = sortOrder switch
            {
                "subject_code_desc" => subjects.OrderByDescending(s => s.SubjectCode),
                "description" => subjects.OrderBy(s => s.Description),
                "description_desc" => subjects.OrderByDescending(s => s.Description),
                _ => subjects.OrderBy(s => s.SubjectCode), // Default sort by subject code ascending
            };

            return View(await subjects.ToListAsync());
        }

        // API endpoint for prerequisite validation
        [HttpGet]
        public async Task<IActionResult> ValidatePrerequisite(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Json(new { isValid = false });

            var exists = await dBContext.Subjects
                .AnyAsync(s => s.SubjectCode == code);

            return Json(new { isValid = exists });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var subject = await dBContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            // Get all subjects except the current one for prerequisites
            var availableSubjects = await dBContext.Subjects
                .Where(s => s.SubjectCode != id)
                .ToListAsync();

            var viewModel = new AddSubjectViewModel
            {
                SubjectCode = subject.SubjectCode,
                Description = subject.Description,
                Units = subject.Units,
                Offering = subject.Offering,
                Category = subject.Category,
                CourseCode = subject.CourseCode,
                CurriculumYear = subject.CurriculumYear,
                PreRequisiteCode = subject.PreRequisiteCode,
                SubjectsTable = availableSubjects
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddSubjectViewModel viewModel)
        {
            if (id != viewModel.SubjectCode)
            {
                return NotFound();
            }

            // Remove validation for SubjectsTable and SearchPreRequisiteCode
            ModelState.Remove("SubjectsTable");
            ModelState.Remove("SearchPreRequisiteCode");

            if (!ModelState.IsValid)
            {
                viewModel.SubjectsTable = await dBContext.Subjects
                    .Where(s => s.SubjectCode != id)
                    .ToListAsync();
                return View(viewModel);
            }

            try
            {
                // Validate PreRequisiteCode if provided
                if (!string.IsNullOrEmpty(viewModel.PreRequisiteCode))
                {
                    var prerequisiteExists = await dBContext.Subjects
                        .AnyAsync(s => s.SubjectCode == viewModel.PreRequisiteCode);

                    if (!prerequisiteExists)
                    {
                        ModelState.AddModelError("PreRequisiteCode", "Invalid prerequisite subject code");
                        viewModel.SubjectsTable = await dBContext.Subjects
                            .Where(s => s.SubjectCode != id)
                            .ToListAsync();
                        return View(viewModel);
                    }

                    if (viewModel.PreRequisiteCode == id)
                    {
                        ModelState.AddModelError("PreRequisiteCode", "A subject cannot be its own prerequisite");
                        viewModel.SubjectsTable = await dBContext.Subjects
                            .Where(s => s.SubjectCode != id)
                            .ToListAsync();
                        return View(viewModel);
                    }
                }

                var subject = await dBContext.Subjects.FindAsync(id);
                if (subject == null)
                {
                    return NotFound();
                }

                // Update the subject properties
                subject.Description = viewModel.Description;
                subject.Units = viewModel.Units;
                subject.Offering = viewModel.Offering;
                subject.Category = viewModel.Category;
                subject.CourseCode = viewModel.CourseCode;
                subject.CurriculumYear = viewModel.CurriculumYear;
                subject.PreRequisiteCode = string.IsNullOrEmpty(viewModel.PreRequisiteCode)
                    ? null
                    : viewModel.PreRequisiteCode;

                dBContext.Entry(subject).State = EntityState.Modified;
                await dBContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Subject updated successfully";
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the subject. Please try again.");
                viewModel.SubjectsTable = await dBContext.Subjects
                    .Where(s => s.SubjectCode != id)
                    .ToListAsync();
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var subject = await dBContext.Subjects
                    .Include(s => s.Schedules)
                        .ThenInclude(ss => ss.EnrollmentDetails)
                    .FirstOrDefaultAsync(s => s.SubjectCode == code);

                if (subject == null)
                {
                    TempData["ErrorMessage"] = "Subject not found.";
                    return RedirectToAction("List");
                }

                // Check if this subject is a prerequisite for other subjects
                var isPrerequisite = await dBContext.Subjects
                    .AnyAsync(s => s.PreRequisiteCode == code);

                if (isPrerequisite)
                {
                    TempData["ErrorMessage"] = "Cannot delete subject as it is a prerequisite for other subjects. Please update those subjects first.";
                    return RedirectToAction("List");
                }

                // Begin transaction
                using var transaction = await dBContext.Database.BeginTransactionAsync();
                try
                {
                    // Remove all related enrollment details first
                    foreach (var schedule in subject.Schedules)
                    {
                        if (schedule.EnrollmentDetails.Any())
                        {
                            var enrollmentDetails = schedule.EnrollmentDetails.ToList();
                            foreach (var detail in enrollmentDetails)
                            {
                                // Get the enrollment header
                                var header = await dBContext.EnrollmentHeaders
                                    .Include(eh => eh.EnrollmentDetails)
                                    .FirstOrDefaultAsync(eh => eh.Id == detail.EnrollmentHeaderId);

                                if (header != null)
                                {
                                    // Update the total units in the header
                                    header.TotalUnits -= subject.Units;
                                    dBContext.EnrollmentDetails.Remove(detail);

                                    // If this was the last subject in the enrollment, remove the header too
                                    if (header.EnrollmentDetails.Count <= 1)  // 1 because current detail isn't removed yet
                                    {
                                        dBContext.EnrollmentHeaders.Remove(header);
                                    }
                                    else
                                    {
                                        dBContext.EnrollmentHeaders.Update(header);
                                    }
                                }
                            }
                        }
                    }

                    // Remove the subject (this will cascade delete schedules due to our DbContext configuration)
                    dBContext.Subjects.Remove(subject);

                    await dBContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Subject and all related records were successfully deleted.";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw; // Re-throw to be caught by outer try-catch
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the subject. Please try again.";
                // Log the exception details here
            }

            return RedirectToAction("List");
        }

    }
}