using System.ComponentModel.DataAnnotations;
using StudentPortal.Models.Entities;

namespace StudentPortal.Models
{
    public class AddSubjectViewModel
    {
        [Required(ErrorMessage = "Subject Code is required")]
        [Display(Name = "Subject Code")]
        [StringLength(15, ErrorMessage = "Subject Code cannot be longer than 15 characters.")]
        public string SubjectCode { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(50, ErrorMessage = "Subject Description cannot be longer than 50 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Units is required")]
        [Range(1, 4, ErrorMessage = "Units must be 1, 2, 3 or 4")]
        public int Units { get; set; }

        [Required(ErrorMessage = "Offering is required")]
        public string Offering { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Course Code is required")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Required(ErrorMessage = "Curriculum Year is required")]
        [Display(Name = "Curriculum Year")]
        public string CurriculumYear { get; set; }

        [Display(Name = "Pre-Requisite")]
        [StringLength(15, ErrorMessage = "Requisite Code cannot be longer than 15 characters.")]
        public string? PreRequisiteCode { get; set; }

        public IEnumerable<Subject>? SubjectsTable { get; set; }
        public string? SearchPreRequisiteCode { get; set; }
    }
}