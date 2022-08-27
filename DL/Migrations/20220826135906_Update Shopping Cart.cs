using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class UpdateShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "ShoppingCartItems");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "MealPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "MealPlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "MealPlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShoppingCartItemMealEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealId = table.Column<long>(type: "bigint", nullable: false),
                    ShoppingCartItemId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItemMealEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItemMealEntity_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItemMealEntity_ShoppingCartItems_ShoppingCartItemId",
                        column: x => x.ShoppingCartItemId,
                        principalTable: "ShoppingCartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealPlans_CreatedById",
                table: "MealPlans",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItemMealEntity_MealId",
                table: "ShoppingCartItemMealEntity",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItemMealEntity_ShoppingCartItemId",
                table: "ShoppingCartItemMealEntity",
                column: "ShoppingCartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_MUser_CreatedById",
                table: "MealPlans",
                column: "CreatedById",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_MUser_CreatedById",
                table: "MealPlans");

            migrationBuilder.DropTable(
                name: "ShoppingCartItemMealEntity");

            migrationBuilder.DropIndex(
                name: "IX_MealPlans_CreatedById",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "MealPlans");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MealPlans");

            migrationBuilder.AddColumn<float>(
                name: "Quantity",
                table: "ShoppingCartItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "ShoppingCartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
