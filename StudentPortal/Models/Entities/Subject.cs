using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models.Entities
{
    public class Subject
    {
        [Key]
        [Required]
        public string SubjectCode { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Range(1, 6)]
        public int Units { get; set; }

        [Required]
        [StringLength(50)]
        public string Offering { get; set; }

        [Required]
        [StringLength(5)]
        public string Category { get; set; }

        [Required]
        [StringLength(20)]
        public string CourseCode { get; set; }

        [Required]
        [StringLength(20)]
        public string CurriculumYear { get; set; }

        public string? PreRequisiteCode { get; set; }

        [ForeignKey("PreRequisiteCode")]
        public virtual Subject? PreRequisite { get; set; }
        public virtual ICollection<SubjectSchedule> Schedules { get; set; } = new List<SubjectSchedule>();
    }
}
