using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductService.Host.Migrations
{
    public partial class Added_ImageName_To_Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "PmProducts",
                maxLength: 128,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "PmProducts");
        }
    }
}
