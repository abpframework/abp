using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.BloggingTestApp.EntityFrameworkCore.Migrations
{
    public partial class Added_SocialLinks_To_Blog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Github",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StackOverflow",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "BlgBlogs");

            migrationBuilder.DropColumn(
                name: "Github",
                table: "BlgBlogs");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "BlgBlogs");

            migrationBuilder.DropColumn(
                name: "StackOverflow",
                table: "BlgBlogs");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "BlgBlogs");
        }
    }
}
