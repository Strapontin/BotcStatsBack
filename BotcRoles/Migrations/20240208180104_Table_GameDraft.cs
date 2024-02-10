using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class Table_GameDraft : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GameDraftId",
                table: "UpdateHistories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GamesDraft",
                columns: table => new
                {
                    GameDraftId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EditionId = table.Column<long>(type: "bigint", nullable: false),
                    StorytellerId = table.Column<long>(type: "bigint", nullable: false),
                    DatePlayed = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesDraft", x => x.GameDraftId);
                    table.ForeignKey(
                        name: "FK_GamesDraft_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "EditionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamesDraft_Players_StorytellerId",
                        column: x => x.StorytellerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamesDraft_EditionId",
                table: "GamesDraft",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_GamesDraft_StorytellerId",
                table: "GamesDraft",
                column: "StorytellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesDraft");

            migrationBuilder.DropColumn(
                name: "GameDraftId",
                table: "UpdateHistories");
        }
    }
}
