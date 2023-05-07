using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class WinningAlignmentColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesPlayedByPlayer",
                table: "PlayerRoleGames");

            migrationBuilder.AddColumn<int>(
                name: "WinningAlignment",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinningAlignment",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "TimesPlayedByPlayer",
                table: "PlayerRoleGames",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
