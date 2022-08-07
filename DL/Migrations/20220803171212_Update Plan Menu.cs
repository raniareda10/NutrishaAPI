using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class UpdatePlanMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PlanDayMenus");

            migrationBuilder.AddColumn<bool>(
                name: "IsEaten",
                table: "PlanDayMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSkipped",
                table: "PlanDayMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSwapped",
                table: "PlanDayMenus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEaten",
                table: "PlanDayMenus");

            migrationBuilder.DropColumn(
                name: "IsSkipped",
                table: "PlanDayMenus");

            migrationBuilder.DropColumn(
                name: "IsSwapped",
                table: "PlanDayMenus");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PlanDayMenus",
                type: "int",
                nullable: true);
        }
    }
}
