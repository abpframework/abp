using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.DocsTestApp.EntityFrameworkCore.Migrations
{
    public partial class Removed_GoogleCustomSearchId : Migration
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
