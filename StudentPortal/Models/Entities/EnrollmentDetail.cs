using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models.Entities
{
    public class EnrollmentDetail
    {
        [Key]
        public long Id { get; set; }  // Added primary key

        [Required]
        public long EnrollmentHeaderId { get; set; }

        [Required]
        public long StudentId { get; set; }

        [Required]
        public string EdpCode { get; set; }

        [Required]
        public string SubjectCode { get; set; }

        [StringLength(200)]
        public string? Remarks { get; set; }

        // Navigation properties
        [ForeignKey("EnrollmentHeaderId")]
        public virtual EnrollmentHeader EnrollmentHeader { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("EdpCode")]
        public virtual SubjectSchedule SubjectSchedule { get; set; }

        [ForeignKey("SubjectCode")]
        public virtual Subject Subject { get; set; }
    }
}
