using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using StudentPortal.Data;
using StudentPortal.Models.Entities;
using StudentPortal.Models;
using Microsoft.AspNetCore.Authorization;

namespace StudentPortal.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDBContext _context;

        public EnrollmentController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new EnrollmentViewModel
            {
                AvailableSchedules = _context.SubjectSchedules
                    .Include(s => s.Subject)
                    .ToList()
            };
            return View(viewModel);
        }

        // POST: Verify Student
        [HttpPost]
        public async Task<IActionResult> VerifyStudent(long studentId)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            return Json(new
            {
                success = true,
                student = new
                {
                    name = $"{student.FirstName} {student.LastName}",
                    course = student.Course,
                    year = student.Year
                }
            });
        }

        // POST: Add Subject
        [HttpPost]
        public async Task<IActionResult> AddSubject(long studentId, string edpCode)
        {
            var schedule = await _context.SubjectSchedules
                .Include(s => s.Subject)
                    .ThenInclude(s => s.PreRequisite)
                .FirstOrDefaultAsync(s => s.EDPCode == edpCode);

            if (schedule == null)
            {
                return Json(new { success = false, message = "Schedule not found" });
            }

            // Check if student exists and include their year level
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            // Validate year level requirements
            if (schedule.SchoolYear > student.Year)
            {
                return Json(new
                {
                    success = false,
                    message = $"This subject is for Year {schedule.SchoolYear} students. You are currently in Year {student.Year}."
                });
            }

            // Check prerequisites
            if (!string.IsNullOrEmpty(schedule.Subject.PreRequisiteCode))
            {
                var hasCompletedPrereq = await _context.EnrollmentDetails
                    .AnyAsync(ed => ed.StudentId == studentId &&
                                   ed.SubjectCode == schedule.Subject.PreRequisiteCode);

                if (!hasCompletedPrereq)
                {
                    var prereqSubject = await _context.Subjects
                        .FirstOrDefaultAsync(s => s.SubjectCode == schedule.Subject.PreRequisiteCode);
                    return Json(new
                    {
                        success = false,
                        message = $"You must complete {prereqSubject?.SubjectCode} ({prereqSubject?.Description}) first."
                    });
                }
            }

            // Check for schedule conflicts
            var existingSchedules = await _context.EnrollmentDetails
                .Where(ed => ed.StudentId == studentId)
                .Include(ed => ed.SubjectSchedule)
                .ToListAsync();

            foreach (var existing in existingSchedules)
            {
                if (HasScheduleConflict(schedule, existing.SubjectSchedule))
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Schedule conflicts with {existing.SubjectCode}"
                    });
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get or create enrollment header
                var header = await _context.EnrollmentHeaders
                    .FirstOrDefaultAsync(h => h.StudentId == studentId &&
                                            h.SchoolYear == schedule.SchoolYear);

                if (header == null)
                {
                    header = new EnrollmentHeader
                    {
                        StudentId = studentId,
                        SchoolYear = schedule.SchoolYear,
                        EnrollmentDate = DateTime.Now,
                        Encoder = User.Identity?.Name ?? "System",
                        TotalUnits = 0
                    };
                    _context.EnrollmentHeaders.Add(header);
                    await _context.SaveChangesAsync();
                }

                // Check total units
                var newTotalUnits = header.TotalUnits + schedule.Subject.Units;
                if (newTotalUnits > EnrollmentViewModel.MaxUnits)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Maximum units ({EnrollmentViewModel.MaxUnits}) exceeded"
                    });
                }

                // Check for existing enrollment
                var existingEnrollment = await _context.EnrollmentDetails
                    .AnyAsync(d => d.StudentId == studentId && d.EdpCode == edpCode);

                if (existingEnrollment)
                {
                    return Json(new { success = false, message = "Subject already enrolled" });
                }

                // Add enrollment detail
                var detail = new EnrollmentDetail
                {
                    EnrollmentHeaderId = header.Id,
                    StudentId = studentId,
                    EdpCode = edpCode,
                    SubjectCode = schedule.SubjectCode
                };

                _context.EnrollmentDetails.Add(detail);
                header.TotalUnits = newTotalUnits;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new
                {
                    success = true,
                    totalUnits = newTotalUnits,
                    subject = new
                    {
                        edpCode = schedule.EDPCode,
                        subjectCode = schedule.SubjectCode,
                        startTime = schedule.StartTime.ToString("hh:mm tt"),
                        endTime = schedule.EndTime.ToString("hh:mm tt"),
                        days = schedule.Days,
                        units = schedule.Subject.Units
                    }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Error saving enrollment: " + ex.Message });
            }
        }

        private bool HasScheduleConflict(SubjectSchedule schedule1, SubjectSchedule schedule2)
        {
            // If days don't overlap, there's no conflict
            if (!HasDayOverlap(schedule1.Days, schedule2.Days))
                return false;

            // Check time overlap
            return schedule1.StartTime < schedule2.EndTime &&
                   schedule2.StartTime < schedule1.EndTime;
        }

        private bool HasDayOverlap(string days1, string days2)
        {
            return days1.Any(day => days2.Contains(day));
        }

        // POST: Remove Subject
        [HttpPost]
        public async Task<IActionResult> RemoveSubject(long studentId, string edpCode)
        {
            var enrollment = await _context.EnrollmentDetails
                .Include(d => d.SubjectSchedule)
                .Include(d => d.SubjectSchedule.Subject)
                .FirstOrDefaultAsync(d => d.StudentId == studentId &&
                                        d.EdpCode == edpCode);

            if (enrollment == null)
            {
                return Json(new { success = false, message = "Enrollment not found" });
            }

            var header = await _context.EnrollmentHeaders
                .FirstOrDefaultAsync(h => h.StudentId == studentId);

            if (header != null)
            {
                header.TotalUnits -= enrollment.SubjectSchedule.Subject.Units;
            }

            _context.EnrollmentDetails.Remove(enrollment);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                totalUnits = header?.TotalUnits ?? 0
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetEnrolledSubjects(long studentId)
        {
            var enrolledSubjects = await _context.EnrollmentDetails
                .Include(d => d.SubjectSchedule)
                    .ThenInclude(s => s.Subject)
                .Where(d => d.StudentId == studentId)
                .Select(d => new
                {
                    edpCode = d.EdpCode,
                    subjectCode = d.SubjectCode,
                    days = d.SubjectSchedule.Days,
                    startTime = d.SubjectSchedule.StartTime.ToString("hh:mm tt"),
                    endTime = d.SubjectSchedule.EndTime.ToString("hh:mm tt"),
                    units = d.SubjectSchedule.Subject.Units
                })
                .ToListAsync();

            var totalUnits = await _context.EnrollmentHeaders
                .Where(h => h.StudentId == studentId)
                .Select(h => h.TotalUnits)
                .FirstOrDefaultAsync();

            return Json(new
            {
                success = true,
                subjects = enrolledSubjects,
                totalUnits = totalUnits
            });
        }
    }
}
