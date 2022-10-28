using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class UserMessageDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasNewMessage",
                table: "MUser",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastMessage",
                table: "MUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasNewMessage",
                table: "MUser");

            migrationBuilder.DropColumn(
                name: "LastMessage",
                table: "MUser");
        }
    }
}
