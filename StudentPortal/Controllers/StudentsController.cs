using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using System.Data.Common;

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
        public async Task <IActionResult> Add(AddStudentViewModel viewModel)
        {
            var student = new Student
            {
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                LastName = viewModel.LastName,
                Course = viewModel.Course,
                Year = viewModel.Year,
            };
            await dBContext.Students.AddAsync(student);
            await dBContext.SaveChangesAsync();

            return RedirectToAction("List", "Students");
        }

        [HttpGet]
        public async Task <IActionResult> List()
        {
            var students = await dBContext.Students.ToListAsync();

            return View(students);
        }

        [HttpGet]
        public async Task <IActionResult> Edit(Guid id)
        {
            var student = await dBContext.Students.FindAsync(id);

            return View(student);
        }

        [HttpPost]
        public async Task <IActionResult> Edit(Student viewModel)
        {
            var student = await dBContext.Students.FindAsync(viewModel.Id);

            if (student is not null)
            {
                student.FirstName = viewModel.FirstName;
                student.MiddleName = viewModel.MiddleName;
                student.LastName = viewModel.LastName;
                student.Course = viewModel.Course;
                student.Year = viewModel.Year;

                await dBContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task <IActionResult> Delete(Student viewModel)
        {
            var student = await dBContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);

            if (student is not null)
            {
                dBContext.Students.Remove(viewModel);
                await dBContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
    }
}
