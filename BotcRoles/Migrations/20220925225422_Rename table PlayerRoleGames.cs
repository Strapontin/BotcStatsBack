using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class RenametablePlayerRoleGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Games_GameId",
                table: "PlayerRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Players_PlayerId",
                table: "PlayerRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Roles_RoleId",
                table: "PlayerRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerRoles",
                table: "PlayerRoles");

            migrationBuilder.RenameTable(
                name: "PlayerRoles",
                newName: "PlayerRoleGames");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRoles_RoleId",
                table: "PlayerRoleGames",
                newName: "IX_PlayerRoleGames_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRoles_PlayerId",
                table: "PlayerRoleGames",
                newName: "IX_PlayerRoleGames_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRoles_GameId",
                table: "PlayerRoleGames",
                newName: "IX_PlayerRoleGames_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerRoleGames",
                table: "PlayerRoleGames",
                column: "PlayerRoleGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoleGames_Games_GameId",
                table: "PlayerRoleGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoleGames_Players_PlayerId",
                table: "PlayerRoleGames",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoleGames_Roles_RoleId",
                table: "PlayerRoleGames",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoleGames_Games_GameId",
                table: "PlayerRoleGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoleGames_Players_PlayerId",
                table: "PlayerRoleGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoleGames_Roles_RoleId",
                table: "PlayerRoleGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerRoleGames",
                table: "PlayerRoleGames");

            migrationBuilder.RenameTable(
                name: "PlayerRoleGames",
                newName: "PlayerRoles");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRoleGames_RoleId",
                table: "PlayerRoles",
                newName: "IX_PlayerRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRoleGames_PlayerId",
                table: "PlayerRoles",
                newName: "IX_PlayerRoles_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerRoleGames_GameId",
                table: "PlayerRoles",
                newName: "IX_PlayerRoles_GameId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerRoles",
                table: "PlayerRoles",
                column: "PlayerRoleGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Games_GameId",
                table: "PlayerRoles",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Players_PlayerId",
                table: "PlayerRoles",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Roles_RoleId",
                table: "PlayerRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId");
        }
    }
}
