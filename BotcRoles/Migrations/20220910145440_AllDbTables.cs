using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class AllDbTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Alignment",
                table: "Roles",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "PlayerRoles",
                newName: "GameId");

            migrationBuilder.AddColumn<int>(
                name: "DefaultAlignment",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinalAlignment",
                table: "PlayerRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StoryTellerPlayerId = table.Column<long>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Game_Module_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Module",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Game_Players_StoryTellerPlayerId",
                        column: x => x.StoryTellerPlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRoles_GameId",
                table: "PlayerRoles",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ModuleId",
                table: "Game",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_StoryTellerPlayerId",
                table: "Game",
                column: "StoryTellerPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Game_GameId",
                table: "PlayerRoles",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Game_GameId",
                table: "PlayerRoles");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropIndex(
                name: "IX_PlayerRoles_GameId",
                table: "PlayerRoles");

            migrationBuilder.DropColumn(
                name: "DefaultAlignment",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "FinalAlignment",
                table: "PlayerRoles");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Roles",
                newName: "Alignment");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "PlayerRoles",
                newName: "Count");
        }
    }
}
