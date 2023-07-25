using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class Column_Games_DatePlayed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Games",
                newName: "DatePlayed");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "DatePlayed",
                table: "Games",
                newName: "CreationDate");
        }
    }
}
