using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class Rename_Storyteller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoryTellerId",
                table: "Games",
                newName: "StorytellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorytellerId",
                table: "Games",
                newName: "StoryTellerId");
        }
    }
}
