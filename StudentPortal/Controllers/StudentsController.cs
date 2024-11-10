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

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            // First check if the ID already exists
            var existingStudent = await dBContext.Students.FirstOrDefaultAsync(s => s.Id == viewModel.StudentId);
            if (existingStudent != null)
            {
                ModelState.AddModelError("StudentId", "This Student ID already exists. Please use a different ID.");
                return View(viewModel);
            }

            // Then check other validations
            if (!ModelState.IsValid)
            {
                return View(viewModel);
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
                return RedirectToAction("List", "Students");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("StudentId", "An error occurred while saving. The Student ID may be duplicate.");
                return View(viewModel);
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
                return View(viewModel);
            }

            try
            {
                // Remove the existing student
                var existingStudent = await dBContext.Students.FindAsync(viewModel.StudentId);
                if (existingStudent != null)
                {
                    // Update the existing student's properties
                    existingStudent.FirstName = viewModel.FirstName;
                    existingStudent.MiddleName = viewModel.MiddleName;
                    existingStudent.LastName = viewModel.LastName;
                    existingStudent.Course = viewModel.Course;
                    existingStudent.Year = viewModel.Year;
                    existingStudent.Remarks = viewModel.Remarks;

                    await dBContext.SaveChangesAsync();
                    return RedirectToAction("List", "Students");
                }

                return NotFound();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error occurred while updating the student.");
                return View(viewModel);
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
    }
}