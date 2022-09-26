using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class AddUserSubscriptionPersonalFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityLevel",
                table: "MUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EatReason",
                table: "MUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasBaby",
                table: "MUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRegularMeasurer",
                table: "MUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MedicineNames",
                table: "MUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberOfMealsPerDay",
                table: "MUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TargetWeight",
                table: "MUser",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityLevel",
                table: "MUser");

            migrationBuilder.DropColumn(
                name: "EatReason",
                table: "MUser");

            migrationBuilder.DropColumn(
                name: "HasBaby",
                table: "MUser");

            migrationBuilder.DropColumn(
                name: "IsRegularMeasurer",
                table: "MUser");

            migrationBuilder.DropColumn(
                name: "MedicineNames",
                table: "MUser");

            migrationBuilder.DropColumn(
                name: "NumberOfMealsPerDay",
                table: "MUser");

            migrationBuilder.DropColumn(
                name: "TargetWeight",
                table: "MUser");
        }
    }
}
