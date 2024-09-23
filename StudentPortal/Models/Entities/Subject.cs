using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class Subject
    {
        [Key]
        [Required]
        public string SubjectCode { get; set; }
        public string Description { get; set; }
        public string Units { get; set; }
        public string Offering { get; set; }
        public string Category { get; set; }
        public string CourseCode { get; set; }
        public string CurriculumYear { get; set; }
    }
}
