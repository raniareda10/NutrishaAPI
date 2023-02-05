using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class FixResetPasswordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetUserPassword_AdminUsers_UserId",
                table: "ResetUserPassword");

            migrationBuilder.DropIndex(
                name: "IX_ResetUserPassword_UserId",
                table: "ResetUserPassword");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ResetUserPassword");

            migrationBuilder.CreateIndex(
                name: "IX_ResetUserPassword_AdminUserId",
                table: "ResetUserPassword",
                column: "AdminUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetUserPassword_AdminUsers_AdminUserId",
                table: "ResetUserPassword",
                column: "AdminUserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetUserPassword_AdminUsers_AdminUserId",
                table: "ResetUserPassword");

            migrationBuilder.DropIndex(
                name: "IX_ResetUserPassword_AdminUserId",
                table: "ResetUserPassword");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ResetUserPassword",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResetUserPassword_UserId",
                table: "ResetUserPassword",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetUserPassword_AdminUsers_UserId",
                table: "ResetUserPassword",
                column: "UserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
