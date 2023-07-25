using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class Table_DemonBluff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemonBluffs",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false),
                    GameId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemonBluffs", x => new { x.RoleId, x.GameId });
                    table.ForeignKey(
                        name: "FK_DemonBluffs_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemonBluffs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemonBluffs_GameId",
                table: "DemonBluffs",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_DemonBluffs_RoleId_GameId",
                table: "DemonBluffs",
                columns: new[] { "RoleId", "GameId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemonBluffs");
        }
    }
}
