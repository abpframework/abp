using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Updates_20180906 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReadCount",
                table: "BlgPosts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BlgComments",
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
                    PostId = table.Column<Guid>(nullable: false),
                    RepliedCommentId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlgComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlgComments_BlgPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "BlgPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlgComments_BlgComments_RepliedCommentId",
                        column: x => x.RepliedCommentId,
                        principalTable: "BlgComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlgTags",
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
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    UsageCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlgTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlgPostTags",
                columns: table => new
                {
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    PostId = table.Column<Guid>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlgPostTags", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlgPostTags_BlgPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "BlgPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlgPostTags_BlgTags_TagId",
                        column: x => x.TagId,
                        principalTable: "BlgTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlgComments_PostId",
                table: "BlgComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlgComments_RepliedCommentId",
                table: "BlgComments",
                column: "RepliedCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlgPostTags_TagId",
                table: "BlgPostTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlgComments");

            migrationBuilder.DropTable(
                name: "BlgPostTags");

            migrationBuilder.DropTable(
                name: "BlgTags");

            migrationBuilder.DropColumn(
                name: "ReadCount",
                table: "BlgPosts");
        }
    }
}
