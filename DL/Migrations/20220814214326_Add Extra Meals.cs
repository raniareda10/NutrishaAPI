using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class AddExtraMeals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanDayMenuMeals_Meals_MealId",
                table: "PlanDayMenuMeals");

            migrationBuilder.AlterColumn<long>(
                name: "MealId",
                table: "PlanDayMenuMeals",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "MealName",
                table: "PlanDayMenuMeals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDayMenuMeals_Meals_MealId",
                table: "PlanDayMenuMeals",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanDayMenuMeals_Meals_MealId",
                table: "PlanDayMenuMeals");

            migrationBuilder.DropColumn(
                name: "MealName",
                table: "PlanDayMenuMeals");

            migrationBuilder.AlterColumn<long>(
                name: "MealId",
                table: "PlanDayMenuMeals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanDayMenuMeals_Meals_MealId",
                table: "PlanDayMenuMeals",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
