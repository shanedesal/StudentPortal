using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models.Entities
{
    public class EnrollmentHeader
    {
        [Key]
        public long Id { get; set; }  // Added primary key

        [Required]
        public long StudentId { get; set; }

        [Required]
        [Range(1,4)]
        public int SchoolYear { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Encoder { get; set; }

        [Required]
        [Range(0, 30)]
        public int TotalUnits { get; set; }

        [StringLength(200)]
        public string? Remarks { get; set; }

        // Navigation properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        public virtual ICollection<EnrollmentDetail> EnrollmentDetails { get; set; } = new List<EnrollmentDetail>();
    }
}
