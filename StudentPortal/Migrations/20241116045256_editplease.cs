using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class editplease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails",
                column: "EdpCode",
                principalTable: "SubjectSchedules",
                principalColumn: "EDPCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentDetails_SubjectSchedules_EdpCode",
                table: "EnrollmentDetails",
                column: "EdpCode",
                principalTable: "SubjectSchedules",
                principalColumn: "EDPCode");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentHeaders_Students_StudentId",
                table: "EnrollmentHeaders",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
