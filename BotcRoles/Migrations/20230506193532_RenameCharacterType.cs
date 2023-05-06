using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class RenameCharacterType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Roles",
                newName: "CharacterType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CharacterType",
                table: "Roles",
                newName: "Type");
        }
    }
}
