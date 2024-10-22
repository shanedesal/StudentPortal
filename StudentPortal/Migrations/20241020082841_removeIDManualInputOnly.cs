using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class removeIDManualInputOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop Primary Key Constraint
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            // Drop the existing Id column with identity
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            // Add new Id column without the IDENTITY property
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Students",
                type: "bigint",
                nullable: false);

            // Re-add Primary Key to the new Id column
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

            // Rollback: Re-add the original Id column with identity
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            // Rollback: Re-add Primary Key to the old Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }
    }
}
