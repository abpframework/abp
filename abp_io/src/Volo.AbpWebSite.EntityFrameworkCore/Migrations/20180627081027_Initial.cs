using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocsProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ShortName = table.Column<string>(maxLength: 32, nullable: false),
                    Format = table.Column<string>(nullable: true),
                    DefaultDocumentName = table.Column<string>(maxLength: 128, nullable: false),
                    NavigationDocumentName = table.Column<string>(maxLength: 128, nullable: false),
                    DocumentStoreType = table.Column<string>(nullable: true),
                    GoogleCustomSearchId = table.Column<string>(nullable: true),
                    ExtraProperties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocsProjects", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocsProjects");
        }
    }
}
