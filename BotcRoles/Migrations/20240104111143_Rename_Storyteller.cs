using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotcRoles.Migrations
{
    public partial class Rename_Storyteller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string renameColumn = @"
                DO $$
                BEGIN
	                IF EXISTS(SELECT *
		                FROM information_schema.columns
		                WHERE table_name='Games' and column_name='StoryTellerId')
	                THEN
	                  ALTER TABLE ""public"".""Games"" RENAME COLUMN ""StoryTellerId"" TO ""StorytellerId"";
	                END IF;
                END $$;";

            migrationBuilder.Sql(renameColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorytellerId",
                table: "Games",
                newName: "StoryTellerId");
        }
    }
}
