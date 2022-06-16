using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class AddTimeTicksToReminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TimeTicks",
                table: "Reminders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTicks",
                table: "Reminders");
        }
    }
}
