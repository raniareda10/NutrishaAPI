using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class AddAdminUserTasbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_MUser_OwnerId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_MUser_CreatedById",
                table: "MealPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MUserRoles_MUser_UserId",
                table: "MUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ResetUserPassword_MUser_UserId",
                table: "ResetUserPassword");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MUserRoles",
                newName: "AdminUserId");

            migrationBuilder.RenameIndex(
                name: "IX_MUserRoles_UserId",
                table: "MUserRoles",
                newName: "IX_MUserRoles_AdminUserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ResetUserPassword",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AdminUserId",
                table: "ResetUserPassword",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Email",
                table: "AdminUsers",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_AdminUsers_OwnerId",
                table: "Blogs",
                column: "OwnerId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_AdminUsers_CreatedById",
                table: "MealPlans",
                column: "CreatedById",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MUserRoles_AdminUsers_AdminUserId",
                table: "MUserRoles",
                column: "AdminUserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResetUserPassword_AdminUsers_UserId",
                table: "ResetUserPassword",
                column: "UserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_AdminUsers_OwnerId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MealPlans_AdminUsers_CreatedById",
                table: "MealPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MUserRoles_AdminUsers_AdminUserId",
                table: "MUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ResetUserPassword_AdminUsers_UserId",
                table: "ResetUserPassword");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "ResetUserPassword");

            migrationBuilder.RenameColumn(
                name: "AdminUserId",
                table: "MUserRoles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MUserRoles_AdminUserId",
                table: "MUserRoles",
                newName: "IX_MUserRoles_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ResetUserPassword",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_MUser_OwnerId",
                table: "Blogs",
                column: "OwnerId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealPlans_MUser_CreatedById",
                table: "MealPlans",
                column: "CreatedById",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MUserRoles_MUser_UserId",
                table: "MUserRoles",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResetUserPassword_MUser_UserId",
                table: "ResetUserPassword",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
