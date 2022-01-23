using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volo.BloggingTestApp.EntityFrameworkCore.Migrations
{
    public partial class IsActiveonmodules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BlgUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BlgUsers");
        }
    }
}
