using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class SupportPlanTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_MUser_UserId",
                table: "MealPlans");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MealPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsTemplate",
                table: "MealPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "MealPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_MUser_UserId",
                table: "MealPlans",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_MUser_UserId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "IsTemplate",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "MealPlans");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "MealPlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_MUser_UserId",
                table: "MealPlans",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
