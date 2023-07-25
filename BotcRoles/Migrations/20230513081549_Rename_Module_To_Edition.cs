using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class Rename_Module_To_Edition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Modules_ModuleId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "RolesModule");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Games",
                newName: "EditionId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_ModuleId",
                table: "Games",
                newName: "IX_Games_EditionId");

            migrationBuilder.CreateTable(
                name: "Editions",
                columns: table => new
                {
                    EditionId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editions", x => x.EditionId);
                });

            migrationBuilder.CreateTable(
                name: "RolesEdition",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false),
                    EditionId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesEdition", x => new { x.RoleId, x.EditionId });
                    table.ForeignKey(
                        name: "FK_RolesEdition_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "EditionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesEdition_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Editions_Name",
                table: "Editions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolesEdition_EditionId",
                table: "RolesEdition",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesEdition_RoleId_EditionId",
                table: "RolesEdition",
                columns: new[] { "RoleId", "EditionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Editions_EditionId",
                table: "Games",
                column: "EditionId",
                principalTable: "Editions",
                principalColumn: "EditionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Editions_EditionId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "RolesEdition");

            migrationBuilder.DropTable(
                name: "Editions");

            migrationBuilder.RenameColumn(
                name: "EditionId",
                table: "Games",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_EditionId",
                table: "Games",
                newName: "IX_Games_ModuleId");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    ModuleId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.ModuleId);
                });

            migrationBuilder.CreateTable(
                name: "RolesModule",
                columns: table => new
                {
                    RoleId = table.Column<long>(type: "INTEGER", nullable: false),
                    ModuleId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesModule", x => new { x.RoleId, x.ModuleId });
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
                name: "IX_Modules_Name",
                table: "Modules",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolesModule_ModuleId",
                table: "RolesModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesModule_RoleId_ModuleId",
                table: "RolesModule",
                columns: new[] { "RoleId", "ModuleId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Modules_ModuleId",
                table: "Games",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
