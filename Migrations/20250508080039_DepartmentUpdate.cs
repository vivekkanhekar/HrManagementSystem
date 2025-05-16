using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "appraisalTemplates");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "appraisalTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplates_DepartmentId",
                table: "appraisalTemplates",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalTemplates_Departments_DepartmentId",
                table: "appraisalTemplates",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_Departments_DepartmentId",
                table: "appraisalTemplates");

            migrationBuilder.DropIndex(
                name: "IX_appraisalTemplates_DepartmentId",
                table: "appraisalTemplates");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "appraisalTemplates");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "appraisalTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
