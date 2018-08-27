using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.BloggingTestApp.EntityFrameworkCore.Migrations
{
    public partial class ReplyComment_Not_Required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlgComments_BlgComments_RepliedCommentId",
                table: "BlgComments");

            migrationBuilder.AlterColumn<Guid>(
                name: "RepliedCommentId",
                table: "BlgComments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_BlgComments_BlgComments_RepliedCommentId",
                table: "BlgComments",
                column: "RepliedCommentId",
                principalTable: "BlgComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlgComments_BlgComments_RepliedCommentId",
                table: "BlgComments");

            migrationBuilder.AlterColumn<Guid>(
                name: "RepliedCommentId",
                table: "BlgComments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlgComments_BlgComments_RepliedCommentId",
                table: "BlgComments",
                column: "RepliedCommentId",
                principalTable: "BlgComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
