using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class Remove_Column_Role_Alignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultAlignment",
                table: "Roles");

            var query = "DELETE FROM 'RolesEdition' " +
                "WHERE 'RoleId' IN (" +
                "   SELECT 'RolesEdition'.'RoleId'" +
                "   FROM public.'RolesEdition'" +
                "   JOIN 'Roles' on 'Roles'.'RoleId' = 'RolesEdition'.'RoleId'" +
                "   WHERE 'Roles'.'CharacterType' IN (4, 5)" +
                ")";
            migrationBuilder.Sql(query.Replace('\'', '"'));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultAlignment",
                table: "Roles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
