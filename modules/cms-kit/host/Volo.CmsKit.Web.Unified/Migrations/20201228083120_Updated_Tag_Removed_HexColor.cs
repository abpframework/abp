using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.CmsKit.Migrations
{
    public partial class Updated_Tag_Removed_HexColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HexColor",
                table: "CmsTags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HexColor",
                table: "CmsTags",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);
        }
    }
}
