using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class TimesheetEmp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimesheetEmployee",
                columns: table => new
                {
                    timeemp_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeparId = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesheetEmployee", x => x.timeemp_Id);
                    table.ForeignKey(
                        name: "FK_TimesheetEmployee_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TimesheetEmployee_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TimesheetEmployee_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TimesheetEmployee_Departments_DeparId",
                        column: x => x.DeparId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetEmployee_AdminId",
                table: "TimesheetEmployee",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetEmployee_ClientId",
                table: "TimesheetEmployee",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetEmployee_DeparId",
                table: "TimesheetEmployee",
                column: "DeparId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesheetEmployee_UserId",
                table: "TimesheetEmployee",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimesheetEmployee");
        }
    }
}
