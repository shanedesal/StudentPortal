using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentPortal.Models.Entities
{
    public class Student
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [StringLength(25)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Range(1, 4)]
        public int Year { get; set; }

        [Required]
        [StringLength(8)]
        public string Course { get; set; }

        [StringLength(50)]
        public string? Remarks { get; set; }

        // Navigation properties
        public virtual EnrollmentHeader? CurrentEnrollment { get; set; }
        public virtual ICollection<EnrollmentHeader> EnrollmentHeaders { get; set; } = new List<EnrollmentHeader>();
        public virtual ICollection<EnrollmentDetail> EnrollmentDetails { get; set; } = new List<EnrollmentDetail>();

    }
}
