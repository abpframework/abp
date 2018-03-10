using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.Abp.EntityFrameworkCore.Tests.Migrations
{
    public partial class Added_IsDeleted_To_Person : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "People",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "People");
        }
    }
}
