using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class AddFavoriteMeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "TakenWaterCupsCount",
                table: "PlanDays",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "UserFavoriteMeals",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MealId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteMeals", x => new { x.MealId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserFavoriteMeals_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteMeals_MUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteMeals_UserId",
                table: "UserFavoriteMeals",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavoriteMeals");

            migrationBuilder.DropColumn(
                name: "TakenWaterCupsCount",
                table: "PlanDays");
        }
    }
}
