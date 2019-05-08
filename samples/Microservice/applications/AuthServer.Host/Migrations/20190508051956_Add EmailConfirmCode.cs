using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Host.Migrations
{
    public partial class AddEmailConfirmCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationCode",
                table: "AbpUsers",
                maxLength: 328,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationCode",
                table: "AbpUsers");
        }
    }
}
