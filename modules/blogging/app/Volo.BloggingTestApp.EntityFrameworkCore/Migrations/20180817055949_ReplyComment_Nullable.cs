using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.BloggingTestApp.EntityFrameworkCore.Migrations
{
    public partial class ReplyComment_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BlgComments_PostId",
                table: "BlgComments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlgComments_BlgPosts_PostId",
                table: "BlgComments",
                column: "PostId",
                principalTable: "BlgPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlgComments_BlgPosts_PostId",
                table: "BlgComments");

            migrationBuilder.DropIndex(
                name: "IX_BlgComments_PostId",
                table: "BlgComments");
        }
    }
}
