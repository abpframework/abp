using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Add_DownloadInfo_CreationDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "Downloads",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreationDuration",
                table: "Downloads",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDuration",
                table: "Downloads");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "Downloads",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
