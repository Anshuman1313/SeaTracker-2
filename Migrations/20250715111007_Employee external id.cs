﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assiginment.Migrations
{
    /// <inheritdoc />
    public partial class Employeeexternalid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Employees");
        }
    }
}
