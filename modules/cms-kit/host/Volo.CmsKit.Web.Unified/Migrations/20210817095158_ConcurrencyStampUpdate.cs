using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.CmsKit.Migrations
{
    public partial class ConcurrencyStampUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CmsComments",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CmsComments");
        }
    }
}
