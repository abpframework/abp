using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.CmsKit.Migrations
{
    public partial class CmsRatings_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CmsRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StarCount = table.Column<short>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsRatings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CmsRatings");
        }
    }
}
