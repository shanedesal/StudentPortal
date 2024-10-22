using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentPortal.Models.Entities
{
    public class Student
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Year { get; set; }
        public string Course { get; set; }

        public string Remarks { get; set; }
    }
}
