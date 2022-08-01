using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DL.Migrations
{
    public partial class FixSomeRolesIssues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MUserRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MUserRoles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MUserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MUserRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MUserRoles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MRoles");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MRoles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MRoles");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "MUserRoles",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "MRoles",
                newName: "Created");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "MUserRoles",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "MRoles",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "MUserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MUserRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MUserRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "MUserRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MUserRoles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "MRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "MRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MRoles",
                type: "datetime2",
                nullable: true);
        }
    }
}
