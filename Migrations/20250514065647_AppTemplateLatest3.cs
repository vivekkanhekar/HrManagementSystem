using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AppTemplateLatest3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appraisalResponses_appraisalTemplates_AppraisalTemplateId",
                table: "appraisalResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_Activities_ActivityId",
                table: "appraisalTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_AspNetUsers_ClientId",
                table: "appraisalTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_AspNetUsers_ManagerId",
                table: "appraisalTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_Departments_DepartmentId",
                table: "appraisalTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appraisalTemplates",
                table: "appraisalTemplates");

            migrationBuilder.RenameTable(
                name: "appraisalTemplates",
                newName: "AppraisalTemplate");

            migrationBuilder.RenameIndex(
                name: "IX_appraisalTemplates_ManagerId",
                table: "AppraisalTemplate",
                newName: "IX_AppraisalTemplate_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_appraisalTemplates_DepartmentId",
                table: "AppraisalTemplate",
                newName: "IX_AppraisalTemplate_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_appraisalTemplates_ClientId",
                table: "AppraisalTemplate",
                newName: "IX_AppraisalTemplate_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_appraisalTemplates_ActivityId",
                table: "AppraisalTemplate",
                newName: "IX_AppraisalTemplate_ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppraisalTemplate",
                table: "AppraisalTemplate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalResponses_AppraisalTemplate_AppraisalTemplateId",
                table: "appraisalResponses",
                column: "AppraisalTemplateId",
                principalTable: "AppraisalTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalTemplate_Activities_ActivityId",
                table: "AppraisalTemplate",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalTemplate_AspNetUsers_ClientId",
                table: "AppraisalTemplate",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalTemplate_AspNetUsers_ManagerId",
                table: "AppraisalTemplate",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalTemplate_Departments_DepartmentId",
                table: "AppraisalTemplate",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appraisalResponses_AppraisalTemplate_AppraisalTemplateId",
                table: "appraisalResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalTemplate_Activities_ActivityId",
                table: "AppraisalTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalTemplate_AspNetUsers_ClientId",
                table: "AppraisalTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalTemplate_AspNetUsers_ManagerId",
                table: "AppraisalTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalTemplate_Departments_DepartmentId",
                table: "AppraisalTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppraisalTemplate",
                table: "AppraisalTemplate");

            migrationBuilder.RenameTable(
                name: "AppraisalTemplate",
                newName: "appraisalTemplates");

            migrationBuilder.RenameIndex(
                name: "IX_AppraisalTemplate_ManagerId",
                table: "appraisalTemplates",
                newName: "IX_appraisalTemplates_ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_AppraisalTemplate_DepartmentId",
                table: "appraisalTemplates",
                newName: "IX_appraisalTemplates_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppraisalTemplate_ClientId",
                table: "appraisalTemplates",
                newName: "IX_appraisalTemplates_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_AppraisalTemplate_ActivityId",
                table: "appraisalTemplates",
                newName: "IX_appraisalTemplates_ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appraisalTemplates",
                table: "appraisalTemplates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalResponses_appraisalTemplates_AppraisalTemplateId",
                table: "appraisalResponses",
                column: "AppraisalTemplateId",
                principalTable: "appraisalTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalTemplates_Activities_ActivityId",
                table: "appraisalTemplates",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalTemplates_AspNetUsers_ClientId",
                table: "appraisalTemplates",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalTemplates_AspNetUsers_ManagerId",
                table: "appraisalTemplates",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalTemplates_Departments_DepartmentId",
                table: "appraisalTemplates",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
