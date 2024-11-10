using StudentPortal.Models.Entities;

namespace StudentPortal.Models
{
    public class EnrollmentViewModel
    {
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public string Course { get; set; }
        public string Year { get; set; }
        public string EDPCode { get; set; }
        public List<SubjectSchedule> AvailableSchedules { get; set; }
        public List<EnrollmentDetail> EnrolledSubjects { get; set; }
        public int TotalUnits { get; set; }
        public const int MaxUnits = 24;
    }
}
