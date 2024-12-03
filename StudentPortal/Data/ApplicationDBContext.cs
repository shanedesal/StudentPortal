using Microsoft.EntityFrameworkCore;
using StudentPortal.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace StudentPortal.Data
{
    public class ApplicationDBContext : IdentityDbContext<Users>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
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
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete for EnrollmentHeaders

                entity.HasMany(s => s.EnrollmentDetails)
                      .WithOne(ed => ed.Student)
                      .HasForeignKey(ed => ed.StudentId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete for EnrollmentDetails
            });

            // Configure Subject entity
            modelBuilder.Entity<Subject>(entity =>
            {
                // PreRequisite relationship
                entity.HasOne(s => s.PreRequisite)
                      .WithMany()
                      .HasForeignKey(s => s.PreRequisiteCode)
                      .OnDelete(DeleteBehavior.NoAction) // NoAction to prevent cycles
                      .IsRequired(false);

                // Schedule relationship
                entity.HasMany(s => s.Schedules)
                      .WithOne(ss => ss.Subject)
                      .HasForeignKey(ss => ss.SubjectCode)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete for Schedules
            });

            // Configure SubjectSchedule entity
            modelBuilder.Entity<SubjectSchedule>(entity =>
            {
                entity.HasMany(ss => ss.EnrollmentDetails)
                      .WithOne(ed => ed.SubjectSchedule)
                      .HasForeignKey(ed => ed.EdpCode)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete for EnrollmentDetails
            });

            // Configure EnrollmentHeader entity
            modelBuilder.Entity<EnrollmentHeader>(entity =>
            {
                entity.HasMany(eh => eh.EnrollmentDetails)
                      .WithOne(ed => ed.EnrollmentHeader)
                      .HasForeignKey(ed => ed.EnrollmentHeaderId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete for EnrollmentDetails

                entity.HasOne(eh => eh.Student)
                      .WithMany(s => s.EnrollmentHeaders)
                      .HasForeignKey(eh => eh.StudentId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete for EnrollmentHeader when Student is deleted
            });

            modelBuilder.Entity<EnrollmentDetail>(entity =>
            {
                // Relationship with Subject - Set to NoAction to avoid multiple cascade paths
                entity.HasOne(ed => ed.Subject)
                      .WithMany()
                      .HasForeignKey(ed => ed.SubjectCode)
                      .OnDelete(DeleteBehavior.NoAction); // Changed to NoAction

                // Relationship with SubjectSchedule
                entity.HasOne(ed => ed.SubjectSchedule)
                      .WithMany(ss => ss.EnrollmentDetails)
                      .HasForeignKey(ed => ed.EdpCode)
                      .OnDelete(DeleteBehavior.Cascade); // Keeps Cascade behavior

                // Relationship with Student
                entity.HasOne(ed => ed.Student)
                      .WithMany(s => s.EnrollmentDetails)
                      .HasForeignKey(ed => ed.StudentId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade to avoid cycles

                // Relationship with EnrollmentHeader
                entity.HasOne(ed => ed.EnrollmentHeader)
                      .WithMany(eh => eh.EnrollmentDetails)
                      .HasForeignKey(ed => ed.EnrollmentHeaderId)
                      .OnDelete(DeleteBehavior.Cascade); // Keeps Cascade behavior

                // Unique index for preventing duplicate enrollments
                entity.HasIndex(ed => new { ed.StudentId, ed.EdpCode })
                      .IsUnique();
            });

        }
    }
}