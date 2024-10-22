using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class foreignKeySubjectCodePrerequisite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PreRequisiteCode",
                table: "Subjects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_PreRequisiteCode",
                table: "Subjects",
                column: "PreRequisiteCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Subjects_PreRequisiteCode",
                table: "Subjects",
                column: "PreRequisiteCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Subjects_PreRequisiteCode",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_PreRequisiteCode",
                table: "Subjects");

            migrationBuilder.AlterColumn<string>(
                name: "PreRequisiteCode",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
