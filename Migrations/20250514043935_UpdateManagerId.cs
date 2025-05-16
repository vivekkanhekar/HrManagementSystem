using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManagerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "appraisalTemplates",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplates_ManagerId",
                table: "appraisalTemplates",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalTemplates_AspNetUsers_ManagerId",
                table: "appraisalTemplates",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_AspNetUsers_ManagerId",
                table: "appraisalTemplates");

            migrationBuilder.DropIndex(
                name: "IX_appraisalTemplates_ManagerId",
                table: "appraisalTemplates");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "appraisalTemplates");
        }
    }
}
