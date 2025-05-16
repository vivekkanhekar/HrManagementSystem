using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ActivityProjectCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Project_Project",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "Project",
                table: "Activities",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_Project",
                table: "Activities",
                newName: "IX_Activities_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Project_ProjectId",
                table: "Activities",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Project_ProjectId",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Activities",
                newName: "Project");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities",
                newName: "IX_Activities_Project");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Project_Project",
                table: "Activities",
                column: "Project",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
