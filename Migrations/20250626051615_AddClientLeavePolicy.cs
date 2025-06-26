using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddClientLeavePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientLeavePolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CasualLeaves = table.Column<int>(type: "int", nullable: false),
                    SickLeaves = table.Column<int>(type: "int", nullable: false),
                    MaternityLeaves = table.Column<int>(type: "int", nullable: false),
                    PrivilegedLeaves = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLeavePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLeavePolicies_AspNetUsers_ClientID",
                        column: x => x.ClientID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientLeavePolicies_ClientID",
                table: "ClientLeavePolicies",
                column: "ClientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientLeavePolicies");
        }
    }
}
