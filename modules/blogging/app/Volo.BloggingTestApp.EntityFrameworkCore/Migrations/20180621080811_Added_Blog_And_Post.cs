using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.BloggingTestApp.EntityFrameworkCore.Migrations
{
    public partial class Added_Blog_And_Post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlgBlogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    ShortName = table.Column<string>(maxLength: 32, nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlgBlogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlgPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    BlogId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 512, nullable: false),
                    Content = table.Column<string>(maxLength: 1048576, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlgPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlgPosts_BlgBlogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "BlgBlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlgPosts_BlogId",
                table: "BlgPosts",
                column: "BlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlgPosts");

            migrationBuilder.DropTable(
                name: "BlgBlogs");
        }
    }
}
