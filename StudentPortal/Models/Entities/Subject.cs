using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models.Entities
{
    public class Subject
    {
        [Key]
        [Required]
        public string SubjectCode { get; set; }
        public string Description { get; set; }
        public int Units { get; set; }
        public string Offering { get; set; }
        public string Category { get; set; }
        public string CourseCode { get; set; }
        public string CurriculumYear { get; set; }
        public string? PreRequisiteCode { get; set; }

        [ForeignKey("PreRequisiteCode")]
        public Subject? PreRequisite { get; set; }
    }
}
