using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class CreateUpdateHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdateHistories",
                columns: table => new
                {
                    UpdateHistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UpdateDetails = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: true),
                    EditionId = table.Column<long>(type: "bigint", nullable: true),
                    GameId = table.Column<long>(type: "bigint", nullable: true)
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpdateHistories_EditionId",
                table: "UpdateHistories",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_UpdateHistories_GameId",
                table: "UpdateHistories",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UpdateHistories_PlayerId",
                table: "UpdateHistories",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_UpdateHistories_RoleId",
                table: "UpdateHistories",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateHistories");
        }
    }
}
