using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class EmpAppraisals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "EmployeeClientAssignments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "EmployeeClientAssignments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "appraisalTemplates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "appraisalResponses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientAssignments_ClientId",
                table: "EmployeeClientAssignments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientAssignments_EmployeeId",
                table: "EmployeeClientAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplates_ActivityId",
                table: "appraisalTemplates",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplates_ClientId",
                table: "appraisalTemplates",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_appraisalResponses_EmployeeId",
                table: "appraisalResponses",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalResponses_AspNetUsers_EmployeeId",
                table: "appraisalResponses",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

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
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientAssignments_AspNetUsers_ClientId",
                table: "EmployeeClientAssignments",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientAssignments_AspNetUsers_EmployeeId",
                table: "EmployeeClientAssignments",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appraisalResponses_AspNetUsers_EmployeeId",
                table: "appraisalResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_Activities_ActivityId",
                table: "appraisalTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_appraisalTemplates_AspNetUsers_ClientId",
                table: "appraisalTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientAssignments_AspNetUsers_ClientId",
                table: "EmployeeClientAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientAssignments_AspNetUsers_EmployeeId",
                table: "EmployeeClientAssignments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeClientAssignments_ClientId",
                table: "EmployeeClientAssignments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeClientAssignments_EmployeeId",
                table: "EmployeeClientAssignments");

            migrationBuilder.DropIndex(
                name: "IX_appraisalTemplates_ActivityId",
                table: "appraisalTemplates");

            migrationBuilder.DropIndex(
                name: "IX_appraisalTemplates_ClientId",
                table: "appraisalTemplates");

            migrationBuilder.DropIndex(
                name: "IX_appraisalResponses_EmployeeId",
                table: "appraisalResponses");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "EmployeeClientAssignments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "EmployeeClientAssignments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "appraisalTemplates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "appraisalResponses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
