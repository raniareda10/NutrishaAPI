using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class SetAppUserIdasNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PaymentHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PaymentHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
