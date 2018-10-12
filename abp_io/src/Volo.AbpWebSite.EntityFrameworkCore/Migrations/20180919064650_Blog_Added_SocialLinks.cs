using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Blog_Added_SocialLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Github",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StackOverflow",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "BlgBlogs",
                maxLength: 128,
                nullable: true);
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
