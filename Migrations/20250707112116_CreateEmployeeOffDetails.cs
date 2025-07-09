using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmployeeOffDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EPFNumber",
                table: "EmployeeOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ESIEnrollmentDate",
                table: "EmployeeOffDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ESINumber",
                table: "EmployeeOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasESI",
                table: "EmployeeOffDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPF",
                table: "EmployeeOffDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PFEnrollmentDate",
                table: "EmployeeOffDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PFNumber",
                table: "EmployeeOffDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PF_UAN",
                table: "EmployeeOffDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPFNumber",
                table: "EmployeeOffDetails");

            migrationBuilder.DropColumn(
                name: "ESIEnrollmentDate",
                table: "EmployeeOffDetails");

            migrationBuilder.DropColumn(
                name: "ESINumber",
                table: "EmployeeOffDetails");

            migrationBuilder.DropColumn(
                name: "HasESI",
                table: "EmployeeOffDetails");

            migrationBuilder.DropColumn(
                name: "HasPF",
                table: "EmployeeOffDetails");

            migrationBuilder.DropColumn(
                name: "PFEnrollmentDate",
                table: "EmployeeOffDetails");

            migrationBuilder.DropColumn(
                name: "PFNumber",
                table: "EmployeeOffDetails");

            migrationBuilder.DropColumn(
                name: "PF_UAN",
                table: "EmployeeOffDetails");
        }
    }
}
