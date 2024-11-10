using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace StudentPortal.Models.Entities
{
    public class SubjectSchedule
    {
        [Key]
        public string EDPCode { get; set; }

        [Required]
        public string SubjectCode { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [StringLength(10)]
        public string Days { get; set; }

        [Required]
        [StringLength(20)]
        public string Room { get; set; }

        [Required]
        [Range(1, 100)]
        public int MaxSize { get; set; }

        [Required]
        [Range(0, 100)]
        public int ClassSize { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        [StringLength(20)]
        public string Section { get; set; }

        [Required]
        [Range(1, 4)]
        public int SchoolYear { get; set; }

        // Navigation properties
        [ForeignKey("SubjectCode")]
        public virtual Subject Subject { get; set; }
        public virtual ICollection<EnrollmentDetail> EnrollmentDetails { get; set; } = new List<EnrollmentDetail>();
    }
}
