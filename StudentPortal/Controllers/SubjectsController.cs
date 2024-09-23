using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models.Entities;
using StudentPortal.Models;

namespace StudentPortal.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly ApplicationDBContext dBContext;
        public SubjectsController(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        [HttpGet]
        public IActionResult Add() 
        {
            return View();
        } 

        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectViewModel viewModel)
        {
            var subject = new Subject
            {
                SubjectCode = viewModel.SubjectCode,
                Description = viewModel.Description,
                Units = viewModel.Units,
                Offering = viewModel.Offering,
                Category = viewModel.Category,
                CourseCode = viewModel.CourseCode,
                CurriculumYear = viewModel.CurriculumYear,
            };
            await dBContext.Subjects.AddAsync(subject);
            await dBContext.SaveChangesAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var subjects = await dBContext.Subjects.ToListAsync();

            return View(subjects);

        }
        
    }
}
