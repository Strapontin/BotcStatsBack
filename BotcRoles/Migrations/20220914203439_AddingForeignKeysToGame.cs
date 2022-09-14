using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class AddingForeignKeysToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_StoryTellerPlayerId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "StoryTellerPlayerId",
                table: "Games",
                newName: "StoryTellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_StoryTellerPlayerId",
                table: "Games",
                newName: "IX_Games_StoryTellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_StoryTellerId",
                table: "Games",
                column: "StoryTellerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_StoryTellerId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "StoryTellerId",
                table: "Games",
                newName: "StoryTellerPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_StoryTellerId",
                table: "Games",
                newName: "IX_Games_StoryTellerPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_StoryTellerPlayerId",
                table: "Games",
                column: "StoryTellerPlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
