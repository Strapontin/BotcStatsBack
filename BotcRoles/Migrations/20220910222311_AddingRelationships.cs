using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class AddingRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Players_Name",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Modules_Name",
                table: "Modules");

            migrationBuilder.AddColumn<long>(
                name: "RoleModuleId",
                table: "RolesModule",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "PlayerRoleGameId",
                table: "PlayerRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolesModule",
                table: "RolesModule",
                column: "RoleModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerRoles",
                table: "PlayerRoles",
                column: "PlayerRoleGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RolesModule",
                table: "RolesModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerRoles",
                table: "PlayerRoles");

            migrationBuilder.DropColumn(
                name: "RoleModuleId",
                table: "RolesModule");

            migrationBuilder.DropColumn(
                name: "PlayerRoleGameId",
                table: "PlayerRoles");

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
        }
    }
}
