using StudentPortal.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class AddSubjectScheduleViewModel
    {
        public AddSubjectScheduleViewModel()
        {
            SubjectsTable = new List<Subject>();
        }

        // EDP Code Properties
        [Required(ErrorMessage = "EDP Code is required")]
        [Display(Name = "EDP Code")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "EDP Code must be exactly 5 characters")]
        public string EDPCode { get; set; }

        // Subject Properties
        [Required(ErrorMessage = "Subject Code is required")]
        [Display(Name = "Subject Code")]
        [StringLength(15, ErrorMessage = "Subject Code cannot exceed 15 characters")]
        public string SubjectCode { get; set; }

        public IEnumerable<Subject> SubjectsTable { get; set; }

        // Schedule Properties
        [Required(ErrorMessage = "Start Time is required")]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Days are required")]
        [RegularExpression(@"^(MWF|TTH|TTHS|MON|TUE|WED|THU|FRI|SAT)$",
        ErrorMessage = "Invalid schedule pattern. Valid patterns are: MWF, TTH, TTHS, or single days (MON,TUE,WED,THU,FRI,SAT)")]
        public string Days { get; set; }

        // Room Properties
        [Required(ErrorMessage = "Room is required")]
        [StringLength(3, ErrorMessage = "Room cannot exceed 3 characters")]
        public string Room { get; set; }

        // Class Size Properties
        [Required(ErrorMessage = "Maximum Size is required")]
        [Display(Name = "Maximum Size")]
        [Range(1, 100, ErrorMessage = "Maximum Size must be between 1 and 100")]
        public int MaxSize { get; set; }

        [Display(Name = "Class Size")]
        [Range(0, 100, ErrorMessage = "Class Size must be between 0 and 100")]
        public int ClassSize { get; set; }

        // Status and Section Properties
        [Required(ErrorMessage = "Status is required")]
        [StringLength(3, ErrorMessage = "Status must be 3 characters")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Section is required")]
        [StringLength(3, ErrorMessage = "Section cannot exceed 3 characters")]
        public string Section { get; set; }

        // Academic Year Property
        [Required(ErrorMessage = "School Year is required")]
        [Range(1,4, ErrorMessage = "Year must be between 1 and 4")]
        public int SchoolYear { get; set; }
    }
}
