using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Year { get; set; }
        public string Course { get; set; }
    }
}
