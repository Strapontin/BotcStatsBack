using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class RemoveRoleModuleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleModuleId",
                table: "RolesModule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RoleModuleId",
                table: "RolesModule",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
