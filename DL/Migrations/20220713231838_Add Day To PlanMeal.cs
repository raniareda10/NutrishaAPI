using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.migrations
{
    public partial class AddDayToPlanMeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MUser_Mobile",
                table: "MUser");

            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "PlanMeals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Mobile",
                table: "MUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "PlanMeals");

            migrationBuilder.AlterColumn<string>(
                name: "Mobile",
                table: "MUser",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MUser_Mobile",
                table: "MUser",
                column: "Mobile",
                unique: true,
                filter: "[Mobile] IS NOT NULL");
        }
    }
}
