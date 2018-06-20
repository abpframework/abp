using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.DocsTestApp.EntityFrameworkCore.Migrations
{
    public partial class Added_GoogleCustomSearchId_To_Project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleCustomSearchId",
                table: "DocsProjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleCustomSearchId",
                table: "DocsProjects");
        }
    }
}
