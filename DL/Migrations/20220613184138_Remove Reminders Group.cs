using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class RemoveRemindersGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_ReminderGroups_ReminderGroupId",
                table: "Reminders");

            migrationBuilder.DropTable(
                name: "ReminderGroups");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_ReminderGroupId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "ReminderGroupId",
                table: "Reminders");

            migrationBuilder.RenameColumn(
                name: "ReminderType",
                table: "Reminders",
                newName: "ReminderGroupType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReminderGroupType",
                table: "Reminders",
                newName: "ReminderType");

            migrationBuilder.AddColumn<long>(
                name: "ReminderGroupId",
                table: "Reminders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ReminderGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderGroups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_ReminderGroupId",
                table: "Reminders",
                column: "ReminderGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_ReminderGroups_ReminderGroupId",
                table: "Reminders",
                column: "ReminderGroupId",
                principalTable: "ReminderGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
