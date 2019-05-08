using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Upgraded_Docs_20181031 : Migration
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

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "AbpUserTokens",
            //    maxLength: 128,
            //    nullable: false,
            //    oldClrType: typeof(string));

            //migrationBuilder.AlterColumn<string>(
            //    name: "LoginProvider",
            //    table: "AbpUserTokens",
            //    maxLength: 64,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 128);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "AbpClaimTypes",
            //    maxLength: 256,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 128);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LatestVersionBranchName",
                table: "DocsProjects");

            migrationBuilder.DropColumn(
                name: "MinimumVersion",
                table: "DocsProjects");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AbpUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AbpUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AbpClaimTypes",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);
        }
    }
}
