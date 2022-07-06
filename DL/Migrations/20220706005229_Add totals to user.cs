using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.migrations
{
    public partial class Addtotalstouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Totals",
                table: "MUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "{\"likes\":0,\"comments\":0}");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Totals",
                table: "MUser");
        }
    }
}
