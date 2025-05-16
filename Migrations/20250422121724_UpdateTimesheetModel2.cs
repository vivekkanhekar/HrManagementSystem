using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimesheetModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Timesheets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_ClientId",
                table: "Timesheets",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timesheets_AspNetUsers_ClientId",
                table: "Timesheets",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timesheets_AspNetUsers_ClientId",
                table: "Timesheets");

            migrationBuilder.DropIndex(
                name: "IX_Timesheets_ClientId",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Timesheets");
        }
    }
}
