using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class ClickedNumberToPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "NumberOfIAmHungryClicked",
                table: "MealPlans",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfIAmHungryClicked",
                table: "MealPlans");
        }
    }
}
