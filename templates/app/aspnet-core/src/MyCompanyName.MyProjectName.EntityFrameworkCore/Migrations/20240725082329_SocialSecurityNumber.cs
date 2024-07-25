using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanyName.MyProjectName.Migrations
{
    /// <inheritdoc />
    public partial class SocialSecurityNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialSecurityNumber",
                table: "AbpUsers",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialSecurityNumber",
                table: "AbpUsers");
        }
    }
}
