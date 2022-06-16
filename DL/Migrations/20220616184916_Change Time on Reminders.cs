using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class ChangeTimeonReminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time1",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "TimeTicks",
                table: "Reminders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time1",
                table: "Reminders",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<long>(
                name: "TimeTicks",
                table: "Reminders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
