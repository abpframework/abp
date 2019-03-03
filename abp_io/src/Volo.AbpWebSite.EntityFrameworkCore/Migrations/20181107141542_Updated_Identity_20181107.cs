using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Updated_Identity_20181107 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AbpRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "AbpRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "AbpRoles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "AbpRoles");
        }
    }
}
