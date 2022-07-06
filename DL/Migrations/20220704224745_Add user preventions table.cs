using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.migrations
{
    public partial class Adduserpreventionstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPreventions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreventionType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreventions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreventions_MUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreventions_PreventionType",
                table: "UserPreventions",
                column: "PreventionType");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreventions_UserId",
                table: "UserPreventions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPreventions");
        }
    }
}
