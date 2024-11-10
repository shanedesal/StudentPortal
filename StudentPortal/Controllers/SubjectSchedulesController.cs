using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
    [Authorize]
    public class SubjectSchedulesController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public SubjectSchedulesController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: SubjectSchedules/Add
        public IActionResult Add()
        {
            var viewModel = new AddSubjectScheduleViewModel
            {
                SubjectsTable = _dbContext.Subjects.Take(10).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddSubjectScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }

            if (model.StartTime >= model.EndTime)
            {
                ModelState.AddModelError("EndTime",
                    model.StartTime == model.EndTime
                        ? "Class schedule start time and end time cannot be the same"
                        : "End time must be later than start time");
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }

            // Check for duplicate EDP Code
            var edpExists = await _dbContext.SubjectSchedules
                .AnyAsync(s => s.EDPCode == model.EDPCode);

            if (edpExists)
            {
                ModelState.AddModelError("EDPCode", "This EDP Code already exists. Please use a different one.");
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }

            // Verify subject exists
            var subjectExists = await _dbContext.Subjects
                .AnyAsync(s => s.SubjectCode == model.SubjectCode);

            if (!subjectExists)
            {
                ModelState.AddModelError("SubjectCode", "Invalid subject code selected");
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }

            var schedule = new SubjectSchedule
            {
                EDPCode = model.EDPCode,
                SubjectCode = model.SubjectCode,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Days = model.Days,
                Room = model.Room,
                MaxSize = model.MaxSize,
                ClassSize = model.ClassSize,
                Status = model.Status,
                Section = model.Section,
                SchoolYear = model.SchoolYear
            };

            try
            {
                await _dbContext.AddAsync(schedule);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Subject schedule added successfully.";
                return RedirectToAction("List", "SubjectSchedules");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }
        }


        // GET: SubjectSchedules/SearchSubjects
        [HttpGet]
        public async Task<IActionResult> SearchSubjects(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                return Json(new List<object>());

            var subjects = await _dbContext.Subjects
                .Where(s => s.SubjectCode.Contains(term.ToUpper()) ||
                            s.Description.Contains(term.ToUpper()))
                .Select(s => new
                {
                    subjectCode = s.SubjectCode,
                    description = s.Description,
                    units = s.Units,
                    offering = s.Offering,
                    category = s.Category
                })
                .Take(10)
                .ToListAsync();

            return Json(subjects);
        }

        // Helper method to create schedule from model
        private static SubjectSchedule CreateScheduleFromModel(AddSubjectScheduleViewModel model)
        {
            return new SubjectSchedule
            {
                EDPCode = model.EDPCode,
                SubjectCode = model.SubjectCode,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Days = model.Days,
                Room = model.Room,
                MaxSize = model.MaxSize,
                ClassSize = model.ClassSize,
                Status = model.Status,
                Section = model.Section,
                SchoolYear = model.SchoolYear
            };
        }
        public async Task<IActionResult> List(string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["EDPCodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "edpcode_desc" : "";
            ViewData["SubjectCodeSortParm"] = sortOrder == "subject" ? "subject_desc" : "subject";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var schedules = from s in _dbContext.SubjectSchedules
                            select s;

            // Apply search filter if provided
            if (!String.IsNullOrEmpty(searchString))
            {
                schedules = schedules.Where(s =>
                    s.EDPCode.Contains(searchString) ||
                    s.SubjectCode.Contains(searchString) ||
                    s.Section.Contains(searchString) ||
                    s.Room.Contains(searchString)
                );
            }

            // Apply sorting
            schedules = sortOrder switch
            {
                "edpcode_desc" => schedules.OrderByDescending(s => s.EDPCode),
                "subject" => schedules.OrderBy(s => s.SubjectCode),
                "subject_desc" => schedules.OrderByDescending(s => s.SubjectCode),
                _ => schedules.OrderBy(s => s.EDPCode),
            };

            // Get the schedules with related subject information
            var schedulesList = await schedules
                .Include(s => s.Subject) // If you have navigation property
                .ToListAsync();

            // Check if any schedules were found
            if (!schedulesList.Any())
            {
                TempData["InfoMessage"] = "No schedules found.";
            }

            return View(schedulesList);
        }

        // Add these actions to your SubjectSchedulesController class

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var schedule = await _dbContext.SubjectSchedules
                .FirstOrDefaultAsync(s => s.EDPCode == id);

            if (schedule == null)
            {
                return NotFound();
            }

            var viewModel = new AddSubjectScheduleViewModel
            {
                EDPCode = schedule.EDPCode,
                SubjectCode = schedule.SubjectCode,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Days = schedule.Days,
                Room = schedule.Room,
                MaxSize = schedule.MaxSize,
                ClassSize = schedule.ClassSize,
                Status = schedule.Status,
                Section = schedule.Section,
                SchoolYear = schedule.SchoolYear,
                SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AddSubjectScheduleViewModel model)
        {
            if (id != model.EDPCode)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }

            // Check if start time and end time are the same
            if (model.StartTime >= model.EndTime)
            {
                ModelState.AddModelError("EndTime",
                    model.StartTime == model.EndTime
                        ? "Class schedule start time and end time cannot be the same"
                        : "End time must be later than start time");
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }

            // Verify subject exists
            var subjectExists = await _dbContext.Subjects
                .AnyAsync(s => s.SubjectCode == model.SubjectCode);

            if (!subjectExists)
            {
                ModelState.AddModelError("SubjectCode", "Invalid subject code selected");
                model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                return View(model);
            }

            var schedule = await _dbContext.SubjectSchedules
                .FirstOrDefaultAsync(s => s.EDPCode == id);

            if (schedule == null)
            {
                return NotFound();
            }

            // Update the schedule properties
            schedule.SubjectCode = model.SubjectCode;
            schedule.StartTime = model.StartTime;
            schedule.EndTime = model.EndTime;
            schedule.Days = model.Days;
            schedule.Room = model.Room;
            schedule.MaxSize = model.MaxSize;
            schedule.ClassSize = model.ClassSize;
            schedule.Status = model.Status;
            schedule.Section = model.Section;
            schedule.SchoolYear = model.SchoolYear;

            try
            {
                _dbContext.Update(schedule);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Subject schedule updated successfully.";
                return RedirectToAction("List", "SubjectSchedules");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ScheduleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                    model.SubjectsTable = await _dbContext.Subjects.Take(10).ToListAsync();
                    return View(model);
                }
            }
        }

        private async Task<bool> ScheduleExists(string id)
        {
            return await _dbContext.SubjectSchedules.AnyAsync(e => e.EDPCode == id);
        }
    }
}
