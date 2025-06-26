using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemarkAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "appraisalResponses",
                newName: "RemarkEntriesCsv");

            migrationBuilder.AddColumn<string>(
                name: "Amount",
                table: "appraisalTemplatesLatest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "AmountEntriesCsv",
                table: "appraisalResponses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "appraisalTemplatesLatest");

            migrationBuilder.DropColumn(
                name: "AmountEntriesCsv",
                table: "appraisalResponses");

            migrationBuilder.RenameColumn(
                name: "RemarkEntriesCsv",
                table: "appraisalResponses",
                newName: "Remarks");
        }
    }
}
