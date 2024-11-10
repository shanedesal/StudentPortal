﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentPortal.Data;

#nullable disable

namespace StudentPortal.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20241110045046_editSchoolYear")]
    partial class editSchoolYear
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("EdpCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("EnrollmentHeaderId")
                        .HasColumnType("bigint");

                    b.Property<string>("Remarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<string>("SubjectCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("EdpCode");

                    b.HasIndex("EnrollmentHeaderId");

                    b.HasIndex("SubjectCode");

                    b.HasIndex("StudentId", "EdpCode")
                        .IsUnique();

                    b.ToTable("EnrollmentDetails");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentHeader", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Encoder")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Remarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("SchoolYear")
                        .HasColumnType("int");

                    b.Property<long>("StudentId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StudentId1")
                        .HasColumnType("bigint");

                    b.Property<int>("TotalUnits")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("StudentId1")
                        .IsUnique()
                        .HasFilter("[StudentId1] IS NOT NULL");

                    b.ToTable("EnrollmentHeaders");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Student", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("Remarks")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Subject", b =>
                {
                    b.Property<string>("SubjectCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CurriculumYear")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Offering")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PreRequisiteCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Units")
                        .HasColumnType("int");

                    b.HasKey("SubjectCode");

                    b.HasIndex("PreRequisiteCode");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectSchedule", b =>
                {
                    b.Property<string>("EDPCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ClassSize")
                        .HasColumnType("int");

                    b.Property<string>("Days")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxSize")
                        .HasColumnType("int");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("SchoolYear")
                        .HasColumnType("int");

                    b.Property<string>("Section")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SubjectCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("EDPCode");

                    b.HasIndex("SubjectCode");

                    b.ToTable("SubjectSchedules");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentDetail", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.SubjectSchedule", "SubjectSchedule")
                        .WithMany("EnrollmentDetails")
                        .HasForeignKey("EdpCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentPortal.Models.Entities.EnrollmentHeader", "EnrollmentHeader")
                        .WithMany("EnrollmentDetails")
                        .HasForeignKey("EnrollmentHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentPortal.Models.Entities.Student", "Student")
                        .WithMany("EnrollmentDetails")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentPortal.Models.Entities.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("EnrollmentHeader");

                    b.Navigation("Student");

                    b.Navigation("Subject");

                    b.Navigation("SubjectSchedule");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentHeader", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.Student", "Student")
                        .WithMany("EnrollmentHeaders")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentPortal.Models.Entities.Student", null)
                        .WithOne("CurrentEnrollment")
                        .HasForeignKey("StudentPortal.Models.Entities.EnrollmentHeader", "StudentId1");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Subject", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.Subject", "PreRequisite")
                        .WithMany()
                        .HasForeignKey("PreRequisiteCode")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("PreRequisite");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectSchedule", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.Subject", "Subject")
                        .WithMany("Schedules")
                        .HasForeignKey("SubjectCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentHeader", b =>
                {
                    b.Navigation("EnrollmentDetails");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Student", b =>
                {
                    b.Navigation("CurrentEnrollment");

                    b.Navigation("EnrollmentDetails");

                    b.Navigation("EnrollmentHeaders");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Subject", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectSchedule", b =>
                {
                    b.Navigation("EnrollmentDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
