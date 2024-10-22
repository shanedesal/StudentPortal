using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class removeGuidStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the Primary Key Constraint on the `Id` column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            // 2. Drop the `Id` column (Guid)
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            // 3. Add a new `Id` column of type `long` WITHOUT the IDENTITY constraint (Manual Input)
            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Students",
                type: "bigint",
                nullable: false);

            // 4. Re-add the Primary Key constraint to the new `Id` column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the Primary Key constraint on the `Id` column
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            // 2. Drop the new `Id` column (long)
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            // 3. Add the old `Id` column back (Guid)
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            // 4. Re-add the Primary Key constraint to the old `Id` column
            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }
    }



}
