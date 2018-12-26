using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class DownloadInfo_TemplateName_And_DatabaseProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "DatabaseProvider",
                table: "Downloads",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "Downloads",
                maxLength: 42,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatabaseProvider",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "Downloads");
        }
    }
}
