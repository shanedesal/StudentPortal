using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class bisagUyabLangLord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_Students_StudentId",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_Subjects_SubjectCode",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Subjects_PreRequisiteCode",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSchedules_Subjects_SubjectCode",
                table: "SubjectSchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_Students_StudentId",
                table: "EnrollmentDetails",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails",
                column: "EdpCode",
                principalTable: "SubjectSchedules",
                principalColumn: "EDPCode");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_Subjects_SubjectCode",
                table: "EnrollmentDetails",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Subjects_PreRequisiteCode",
                table: "Subjects",
                column: "PreRequisiteCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSchedules_Subjects_SubjectCode",
                table: "SubjectSchedules",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_Students_StudentId",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_Subjects_SubjectCode",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Subjects_PreRequisiteCode",
                table: "Subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectSchedules_Subjects_SubjectCode",
                table: "SubjectSchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_Students_StudentId",
                table: "EnrollmentDetails",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails",
                column: "EdpCode",
                principalTable: "SubjectSchedules",
                principalColumn: "EDPCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_Subjects_SubjectCode",
                table: "EnrollmentDetails",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Subjects_PreRequisiteCode",
                table: "Subjects",
                column: "PreRequisiteCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectSchedules_Subjects_SubjectCode",
                table: "SubjectSchedules",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
