using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.Abp.BackgroundJobs.DemoApp.Migrations
{
    public partial class AggragateRoot_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpBackgroundJobs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AbpBackgroundJobs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AbpBackgroundJobs");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AbpBackgroundJobs");
        }
    }
}
