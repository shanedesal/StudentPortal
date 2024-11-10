using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class AddStudentViewModel
    {
        [Required(ErrorMessage = "Student ID is required.")]
        [Range(10000000, 99999999, ErrorMessage = "Student ID must be exactly 8 digits.")]
        public long StudentId { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(30, ErrorMessage = "First name cannot be longer than 30 characters.")]
        public string FirstName { get; set; }
        [StringLength(30, ErrorMessage = "Middle name cannot be longer than 30 characters.")]
        public string? MiddleName { get; set; }
        [StringLength(30, ErrorMessage = "Last name cannot be longer than 30 characters.")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(1, 4, ErrorMessage = "Year must be between 1 and 4.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        public string Course { get; set; }

        [Required(ErrorMessage = "Remarks is required.")]
        public string Remarks { get; set; }
    }
}
