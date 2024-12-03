using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly ApplicationDBContext dBContext;

        public StudentsController(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAddView()
        {
            return PartialView("_AddStudent", new AddStudentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            // First check if the ID already exists
            var existingStudent = await dBContext.Students.FirstOrDefaultAsync(s => s.Id == viewModel.StudentId);
            if (existingStudent != null)
            {
                ModelState.AddModelError("StudentId", "This Student ID already exists. Please use a different ID.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_AddStudent", viewModel);
            }

            var student = new Student
            {
                Id = viewModel.StudentId,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                LastName = viewModel.LastName,
                Course = viewModel.Course,
                Year = viewModel.Year,
                Remarks = viewModel.Remarks
            };

            try
            {
                await dBContext.Students.AddAsync(student);
                await dBContext.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error occurred while saving the student.");
                return PartialView("_AddStudent", viewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var student = await dBContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new AddStudentViewModel
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                Year = student.Year,
                Course = student.Course,
                Remarks = student.Remarks
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddStudentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                var existingStudent = await dBContext.Students.FindAsync(viewModel.StudentId);
                if (existingStudent != null)
                {
                    existingStudent.FirstName = viewModel.FirstName;
                    existingStudent.MiddleName = viewModel.MiddleName;
                    existingStudent.LastName = viewModel.LastName;
                    existingStudent.Course = viewModel.Course;
                    existingStudent.Year = viewModel.Year;
                    existingStudent.Remarks = viewModel.Remarks;

                    await dBContext.SaveChangesAsync();
                    return Json(new { success = true });
                }

                return Json(new { success = false, errors = new[] { "Student not found" } });
            }
            catch (DbUpdateException)
            {
                return Json(new { success = false, errors = new[] { "An error occurred while updating the student" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> List(string sortOrder, string currentFilter, string searchString)
        {
            // Sort parameters
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["FirstNameSortParm"] = sortOrder == "firstName" ? "firstName_desc" : "firstName";
            ViewData["LastNameSortParm"] = sortOrder == "lastName" ? "lastName_desc" : "lastName";
            ViewData["CourseSortParm"] = sortOrder == "course" ? "course_desc" : "course";
            ViewData["YearSortParm"] = sortOrder == "year" ? "year_desc" : "year";

            // Handle search
            if (searchString != null)
            {
                currentFilter = searchString;
            }

            ViewData["CurrentFilter"] = currentFilter;

            var students = dBContext.Students.AsQueryable();

            // Apply search if provided
            if (!String.IsNullOrEmpty(currentFilter))
            {
                students = students.Where(s =>
                    s.Id.ToString().Contains(currentFilter) ||
                    s.FirstName.Contains(currentFilter) ||
                    s.MiddleName.Contains(currentFilter) ||
                    s.LastName.Contains(currentFilter) ||
                    s.Course.Contains(currentFilter) ||
                    s.Year.ToString().Contains(currentFilter)
                );
            }

            // Apply sorting
            students = sortOrder switch
            {
                "id_desc" => students.OrderByDescending(s => s.Id),
                "firstName" => students.OrderBy(s => s.FirstName),
                "firstName_desc" => students.OrderByDescending(s => s.FirstName),
                "lastName" => students.OrderBy(s => s.LastName),
                "lastName_desc" => students.OrderByDescending(s => s.LastName),
                "course" => students.OrderBy(s => s.Course),
                "course_desc" => students.OrderByDescending(s => s.Course),
                "year" => students.OrderBy(s => s.Year),
                "year_desc" => students.OrderByDescending(s => s.Year),
                _ => students.OrderBy(s => s.Id),
            };

            return View(await students.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            using var transaction = await dBContext.Database.BeginTransactionAsync();
            try
            {
                var student = await dBContext.Students
                    .Include(s => s.EnrollmentHeaders)
                        .ThenInclude(eh => eh.EnrollmentDetails)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (student == null)
                {
                    TempData["ErrorMessage"] = "Student not found.";
                    return RedirectToAction("List");
                }

                // Delete enrollment details first
                foreach (var header in student.EnrollmentHeaders)
                {
                    dBContext.EnrollmentDetails.RemoveRange(header.EnrollmentDetails);
                }

                // Delete enrollment headers
                dBContext.EnrollmentHeaders.RemoveRange(student.EnrollmentHeaders);

                // Finally delete the student
                dBContext.Students.Remove(student);

                await dBContext.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Student and all related records were successfully deleted.";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "An error occurred while deleting the student. Please try again.";
                // Log the exception here
                return RedirectToAction("List");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEditView(long id)
        {
            var student = await dBContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new AddStudentViewModel
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                MiddleName = student.MiddleName,
                LastName = student.LastName,
                Year = student.Year,
                Course = student.Course,
                Remarks = student.Remarks
            };

            return PartialView("_EditStudent", viewModel);
        }
    }
}