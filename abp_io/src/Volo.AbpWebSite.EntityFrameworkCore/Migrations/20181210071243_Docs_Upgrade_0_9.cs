using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Docs_Upgrade_0_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleCustomSearchId",
                table: "DocsProjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleCustomSearchId",
                table: "DocsProjects",
                nullable: true);
        }
    }
}
