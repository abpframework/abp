using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class AggregateRoot_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Downloads",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "Downloads",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "DocsProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "BlgUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "BlgTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "BlgTags",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "BlgPosts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "BlgPosts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "BlgComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "BlgComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "BlgBlogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "BlgBlogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AbpRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "DocsProjects");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "BlgUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "BlgPosts");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "BlgPosts");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "BlgComments");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "BlgComments");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "BlgBlogs");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "BlgBlogs");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AbpRoles");
        }
    }
}
