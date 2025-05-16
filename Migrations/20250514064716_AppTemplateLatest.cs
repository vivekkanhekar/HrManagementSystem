using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AppTemplateLatest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appraisalResponses_appraisalTemplatesOLD_AppraisalTemplateId",
                table: "appraisalResponses");

            migrationBuilder.DropTable(
                name: "appraisalTemplatesOLD");

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

            migrationBuilder.CreateTable(
                name: "appraisalTemplatesOLD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeasuringKeys = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appraisalTemplatesOLD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appraisalTemplatesOLD_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appraisalTemplatesOLD_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_appraisalTemplatesOLD_AspNetUsers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_appraisalTemplatesOLD_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplatesOLD_ActivityId",
                table: "appraisalTemplatesOLD",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplatesOLD_ClientId",
                table: "appraisalTemplatesOLD",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplatesOLD_DepartmentId",
                table: "appraisalTemplatesOLD",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_appraisalTemplatesOLD_ManagerId",
                table: "appraisalTemplatesOLD",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_appraisalResponses_appraisalTemplatesOLD_AppraisalTemplateId",
                table: "appraisalResponses",
                column: "AppraisalTemplateId",
                principalTable: "appraisalTemplatesOLD",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
