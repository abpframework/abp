using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volo.CmsKit.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCommentUrlName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentUrl",
                table: "CmsComments",
                newName: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "CmsComments",
                newName: "CommentUrl");
        }
    }
}
