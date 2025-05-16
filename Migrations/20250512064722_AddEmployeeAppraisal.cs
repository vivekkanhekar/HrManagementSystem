using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeAppraisal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedDate",
                table: "appraisalResponses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_appraisalResponses_AppraisalTemplateId",
                table: "appraisalResponses",
                column: "AppraisalTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalResponses_appraisalTemplates_AppraisalTemplateId",
                table: "appraisalResponses",
                column: "AppraisalTemplateId",
                principalTable: "appraisalTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appraisalResponses_appraisalTemplates_AppraisalTemplateId",
                table: "appraisalResponses");

            migrationBuilder.DropIndex(
                name: "IX_appraisalResponses_AppraisalTemplateId",
                table: "appraisalResponses");

            migrationBuilder.DropColumn(
                name: "SubmittedDate",
                table: "appraisalResponses");
        }
    }
}
