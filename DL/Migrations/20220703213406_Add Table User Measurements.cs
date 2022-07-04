using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.migrations
{
    public partial class AddTableUserMeasurements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMeasurements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementType = table.Column<int>(type: "int", nullable: false),
                    MeasurementValue = table.Column<float>(type: "real", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMeasurements_MUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMeasurements_MeasurementType",
                table: "UserMeasurements",
                column: "MeasurementType");

            migrationBuilder.CreateIndex(
                name: "IX_UserMeasurements_UserId",
                table: "UserMeasurements",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMeasurements");
        }
    }
}
