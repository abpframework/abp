using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.DocsTestApp.EntityFrameworkCore.Migrations
{
    public partial class Added_Project_MinimumVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LatestVersionBranchName",
                table: "DocsProjects",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinimumVersion",
                table: "DocsProjects",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestVersionBranchName",
                table: "DocsProjects");

            migrationBuilder.DropColumn(
                name: "MinimumVersion",
                table: "DocsProjects");
        }
    }
}
