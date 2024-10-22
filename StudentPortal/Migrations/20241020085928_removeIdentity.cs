using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class removeIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use raw SQL to drop the primary key constraint
            migrationBuilder.Sql("ALTER TABLE Students DROP CONSTRAINT IF EXISTS PK_Students");

            // Drop the existing Id column with IDENTITY
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            // Add the Id column back without IDENTITY
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Students",
                type: "bigint",
                nullable: false);

            // Re-add the Primary Key constraint on the new Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Use raw SQL to drop the primary key constraint
            migrationBuilder.Sql("ALTER TABLE Students DROP CONSTRAINT IF EXISTS PK_Students");

            // Drop the manually inputted Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            // Add the Id column back with IDENTITY
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Students",
                type: "bigint",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            // Re-add the Primary Key constraint on the Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }
    }


}
