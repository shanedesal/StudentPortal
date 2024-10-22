using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
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
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Return the view with the validation errors
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

            await dBContext.Students.AddAsync(student);
            await dBContext.SaveChangesAsync();

            return RedirectToAction("List", "Students");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await dBContext.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id) // Change Guid to long
        {
            var student = await dBContext.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(); // Handle student not found
            }

            // Return the student as a view model
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
        public async Task<IActionResult> Edit(AddStudentViewModel viewModel) // Use the view model here
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Return the view with validation errors
            }

            var student = await dBContext.Students.FindAsync(viewModel.StudentId); // Use StudentId

            if (student != null)
            {
                student.FirstName = viewModel.FirstName;
                student.MiddleName = viewModel.MiddleName;
                student.LastName = viewModel.LastName;
                student.Course = viewModel.Course;
                student.Year = viewModel.Year;
                student.Remarks = viewModel.Remarks;
                await dBContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id) // Change parameter type to long
        {
            var student = await dBContext.Students.FindAsync(id); // Use FindAsync to get student by ID

            if (student != null)
            {
                dBContext.Students.Remove(student); // Remove the student entity
                await dBContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");
        }
    }
}
