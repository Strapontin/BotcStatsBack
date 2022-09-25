using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class AddedforeignkeyonPlayerRoleNameRoleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Roles_RoleId",
                table: "PlayerRoles");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "PlayerRoles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Roles_RoleId",
                table: "PlayerRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerRoles_Roles_RoleId",
                table: "PlayerRoles");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "PlayerRoles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerRoles_Roles_RoleId",
                table: "PlayerRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
