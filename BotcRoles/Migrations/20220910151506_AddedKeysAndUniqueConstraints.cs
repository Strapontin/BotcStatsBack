using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class AddedKeysAndUniqueConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_Module_ModuleId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Players_StoryTellerPlayerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Game_GameId",
                table: "PlayerRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Module",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Games");

            migrationBuilder.RenameIndex(
                name: "IX_Game_StoryTellerPlayerId",
                table: "Games",
                newName: "IX_Games_StoryTellerPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_ModuleId",
                table: "Games",
                newName: "IX_Games_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "GameId");

            migrationBuilder.CreateTable(
                name: "playersInGames",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_playersInGames_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playersInGames_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolesModule",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_RolesModule_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesModule_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Name",
                table: "Players",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Name",
                table: "Modules",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_playersInGames_GameId",
                table: "playersInGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_playersInGames_PlayerId",
                table: "playersInGames",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesModule_ModuleId",
                table: "RolesModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesModule_RoleId",
                table: "RolesModule",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Modules_ModuleId",
                table: "Games",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_StoryTellerPlayerId",
                table: "Games",
                column: "StoryTellerPlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Games_GameId",
                table: "PlayerRoles",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Modules_ModuleId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_StoryTellerPlayerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Games_GameId",
                table: "PlayerRoles");

            migrationBuilder.DropTable(
                name: "playersInGames");

            migrationBuilder.DropTable(
                name: "RolesModule");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Players_Name",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_Name",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Module");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Game");

            migrationBuilder.RenameIndex(
                name: "IX_Games_StoryTellerPlayerId",
                table: "Game",
                newName: "IX_Game_StoryTellerPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_ModuleId",
                table: "Game",
                newName: "IX_Game_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Module",
                table: "Module",
                column: "ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Module_ModuleId",
                table: "Game",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Players_StoryTellerPlayerId",
                table: "Game",
                column: "StoryTellerPlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Game_GameId",
                table: "PlayerRoles",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
