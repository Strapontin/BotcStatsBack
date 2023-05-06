using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class AddingEntityTypesConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RolesModule",
                table: "RolesModule");

            migrationBuilder.DropIndex(
                name: "IX_RolesModule_RoleId",
                table: "RolesModule");

            migrationBuilder.AlterColumn<long>(
                name: "RoleModuleId",
                table: "RolesModule",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolesModule",
                table: "RolesModule",
                columns: new[] { "RoleId", "ModuleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolesModule_RoleId_ModuleId",
                table: "RolesModule",
                columns: new[] { "RoleId", "ModuleId" },
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolesModule",
                table: "RolesModule");

            migrationBuilder.DropIndex(
                name: "IX_RolesModule_RoleId_ModuleId",
                table: "RolesModule");

            migrationBuilder.DropIndex(
                name: "IX_Players_Name",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Modules_Name",
                table: "Modules");

            migrationBuilder.AlterColumn<long>(
                name: "RoleModuleId",
                table: "RolesModule",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolesModule",
                table: "RolesModule",
                column: "RoleModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesModule_RoleId",
                table: "RolesModule",
                column: "RoleId");
        }
    }
}
