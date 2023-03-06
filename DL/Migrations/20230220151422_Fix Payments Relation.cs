using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class FixPaymentsRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentHistory_MUser_UserId",
                table: "PaymentHistory",
                column: "UserId",
                principalTable: "MUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
