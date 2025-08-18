using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assiginment.Migrations
{
    /// <inheritdoc />
    public partial class DefaultAbsentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Attendance",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Absent",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Present");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Attendance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Attendance",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Attendance",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Attendance");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Attendance",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Present",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Absent");
        }
    }
}
