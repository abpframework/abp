using Microsoft.EntityFrameworkCore.Migrations;

namespace VoloDocs.EntityFrameworkCore.Migrations
{
    public partial class Added_CommitCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommitCount",
                table: "DocsDocumentContributors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitCount",
                table: "DocsDocumentContributors");
        }
    }
}
