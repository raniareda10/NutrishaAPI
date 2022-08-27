using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class AddParentTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentTemplateId",
                table: "MealPlans",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_ParentTemplateId",
                table: "MealPlans",
                column: "ParentTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_MealPlans_ParentTemplateId",
                table: "MealPlans",
                column: "ParentTemplateId",
                principalTable: "MealPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_MealPlans_ParentTemplateId",
                table: "MealPlans");

            migrationBuilder.DropIndex(
                name: "IX_MealPlans_ParentTemplateId",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "ParentTemplateId",
                table: "MealPlans");
        }
    }
}
