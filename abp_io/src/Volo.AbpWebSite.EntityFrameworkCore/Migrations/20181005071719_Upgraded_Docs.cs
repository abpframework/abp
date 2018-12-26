using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Upgraded_Docs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "MainWebsiteUrl",
                table: "DocsProjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AbpClaimTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    Regex = table.Column<string>(maxLength: 512, nullable: true),
                    RegexDescription = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    ValueType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpClaimTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpClaimTypes");

            migrationBuilder.DropColumn(
                name: "MainWebsiteUrl",
                table: "DocsProjects");

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
    }
}
