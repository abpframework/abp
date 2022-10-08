using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations
{
    public partial class ProjectRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "DocsProjects",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "DocsProjects");
        }
    }
}
