using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class MakeTagIdnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogTag_TagId",
                table: "Blogs");

            migrationBuilder.AlterColumn<long>(
                name: "TagId",
                table: "Blogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogTag_TagId",
                table: "Blogs",
                column: "TagId",
                principalTable: "BlogTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_BlogTag_TagId",
                table: "Blogs");

            migrationBuilder.AlterColumn<long>(
                name: "TagId",
                table: "Blogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_BlogTag_TagId",
                table: "Blogs",
                column: "TagId",
                principalTable: "BlogTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
