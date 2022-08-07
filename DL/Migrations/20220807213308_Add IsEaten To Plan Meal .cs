using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class AddIsEatenToPlanMeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEaten",
                table: "PlanDayMenuMeals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEaten",
                table: "PlanDayMenuMeals");
        }
    }
}
