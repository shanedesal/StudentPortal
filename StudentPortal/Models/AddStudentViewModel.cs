using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class AddStudentViewModel
    {
        [Required(ErrorMessage = "Student ID is required.")]
        public long StudentId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        public string Course { get; set; }

        [Required(ErrorMessage = "Remarks is required.")]
        public string Remarks { get; set; }
    }
}
