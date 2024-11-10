using Microsoft.EntityFrameworkCore;
using StudentPortal.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace StudentPortal.Data
{
    public class ApplicationDBContext : IdentityDbContext<Users>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {
            
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectSchedule> SubjectSchedules { get; set; }
        public DbSet<EnrollmentHeader> EnrollmentHeaders { get; set; }
        public DbSet<EnrollmentDetail> EnrollmentDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasMany(s => s.EnrollmentHeaders)
                      .WithOne(eh => eh.Student)
                      .HasForeignKey(eh => eh.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.EnrollmentDetails)
                      .WithOne(ed => ed.Student)
                      .HasForeignKey(ed => ed.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Subject entity
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasOne(s => s.PreRequisite)
                      .WithMany()
                      .HasForeignKey(s => s.PreRequisiteCode)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Schedules)
                      .WithOne(ss => ss.Subject)
                      .HasForeignKey(ss => ss.SubjectCode)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure EnrollmentHeader entity
            modelBuilder.Entity<EnrollmentHeader>(entity =>
            {
                entity.HasMany(eh => eh.EnrollmentDetails)
                      .WithOne(ed => ed.EnrollmentHeader)
                      .HasForeignKey(ed => ed.EnrollmentHeaderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure EnrollmentDetail entity
            modelBuilder.Entity<EnrollmentDetail>(entity =>
            {
                entity.HasOne(ed => ed.SubjectSchedule)
                      .WithMany(ss => ss.EnrollmentDetails)
                      .HasForeignKey(ed => ed.EdpCode)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ed => ed.Subject)
                      .WithMany()
                      .HasForeignKey(ed => ed.SubjectCode)
                      .OnDelete(DeleteBehavior.Restrict);

                // Create unique index to prevent duplicate enrollments
                entity.HasIndex(ed => new { ed.StudentId, ed.EdpCode })
                      .IsUnique();
            });
        }
    }
}
