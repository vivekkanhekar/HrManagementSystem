using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class EmpLeave2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "EmployeeLeaves",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "EmployeeLeaves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "EmployeeLeaves",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "EmployeeLeaves",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EmployeeLeaves");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "EmployeeLeaves");
        }
    }
}
