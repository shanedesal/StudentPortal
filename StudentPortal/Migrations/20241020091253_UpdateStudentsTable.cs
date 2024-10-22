using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the Primary Key constraint on the Id column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            // 2. Drop the existing Id column (which has IDENTITY)
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            // 3. Add the new Id column without IDENTITY
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Students",
                type: "bigint",
                nullable: false);

            // 4. Re-add the Primary Key constraint on the new Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback: Drop Primary Key on Id column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            // Rollback: Drop the manually entered Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            // Rollback: Re-add the original Id column with IDENTITY
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Students",
                type: "bigint",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // Rollback: Re-add Primary Key to the old Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }
    }

}
