using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.CmsKit.Migrations
{
    public partial class BlogPost_Author : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_CreatorId",
                table: "CmsBlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_DeleterId",
                table: "CmsBlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_LastModifierId",
                table: "CmsBlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_CmsBlogPosts_CreatorId",
                table: "CmsBlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_CmsBlogPosts_DeleterId",
                table: "CmsBlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_CmsBlogPosts_LastModifierId",
                table: "CmsBlogPosts");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "CmsBlogPosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CmsBlogPosts_AuthorId",
                table: "CmsBlogPosts",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_AuthorId",
                table: "CmsBlogPosts",
                column: "AuthorId",
                principalTable: "CmsUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_AuthorId",
                table: "CmsBlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_CmsBlogPosts_AuthorId",
                table: "CmsBlogPosts");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "CmsBlogPosts");

            migrationBuilder.CreateIndex(
                name: "IX_CmsBlogPosts_CreatorId",
                table: "CmsBlogPosts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsBlogPosts_DeleterId",
                table: "CmsBlogPosts",
                column: "DeleterId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsBlogPosts_LastModifierId",
                table: "CmsBlogPosts",
                column: "LastModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_CreatorId",
                table: "CmsBlogPosts",
                column: "CreatorId",
                principalTable: "CmsUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_DeleterId",
                table: "CmsBlogPosts",
                column: "DeleterId",
                principalTable: "CmsUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CmsBlogPosts_CmsUsers_LastModifierId",
                table: "CmsBlogPosts",
                column: "LastModifierId",
                principalTable: "CmsUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
