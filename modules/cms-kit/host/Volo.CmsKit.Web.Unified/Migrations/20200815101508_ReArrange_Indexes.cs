using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.CmsKit.Migrations
{
    public partial class ReArrange_Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CmsUserReactions_EntityType_EntityId",
                table: "CmsUserReactions");

            migrationBuilder.DropIndex(
                name: "IX_CmsUserReactions_CreatorId_EntityType_EntityId_ReactionName",
                table: "CmsUserReactions");

            migrationBuilder.DropIndex(
                name: "IX_CmsComments_RepliedCommentId",
                table: "CmsComments");

            migrationBuilder.DropIndex(
                name: "IX_CmsComments_EntityType_EntityId",
                table: "CmsComments");

            migrationBuilder.CreateIndex(
                name: "IX_CmsUsers_TenantId_Email",
                table: "CmsUsers",
                columns: new[] { "TenantId", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsUsers_TenantId_UserName",
                table: "CmsUsers",
                columns: new[] { "TenantId", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserReactions_TenantId_EntityType_EntityId_ReactionName",
                table: "CmsUserReactions",
                columns: new[] { "TenantId", "EntityType", "EntityId", "ReactionName" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserReactions_TenantId_CreatorId_EntityType_EntityId_ReactionName",
                table: "CmsUserReactions",
                columns: new[] { "TenantId", "CreatorId", "EntityType", "EntityId", "ReactionName" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsComments_TenantId_RepliedCommentId",
                table: "CmsComments",
                columns: new[] { "TenantId", "RepliedCommentId" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsComments_TenantId_EntityType_EntityId",
                table: "CmsComments",
                columns: new[] { "TenantId", "EntityType", "EntityId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CmsUsers_TenantId_Email",
                table: "CmsUsers");

            migrationBuilder.DropIndex(
                name: "IX_CmsUsers_TenantId_UserName",
                table: "CmsUsers");

            migrationBuilder.DropIndex(
                name: "IX_CmsUserReactions_TenantId_EntityType_EntityId_ReactionName",
                table: "CmsUserReactions");

            migrationBuilder.DropIndex(
                name: "IX_CmsUserReactions_TenantId_CreatorId_EntityType_EntityId_ReactionName",
                table: "CmsUserReactions");

            migrationBuilder.DropIndex(
                name: "IX_CmsComments_TenantId_RepliedCommentId",
                table: "CmsComments");

            migrationBuilder.DropIndex(
                name: "IX_CmsComments_TenantId_EntityType_EntityId",
                table: "CmsComments");

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserReactions_EntityType_EntityId",
                table: "CmsUserReactions",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserReactions_CreatorId_EntityType_EntityId_ReactionName",
                table: "CmsUserReactions",
                columns: new[] { "CreatorId", "EntityType", "EntityId", "ReactionName" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsComments_RepliedCommentId",
                table: "CmsComments",
                column: "RepliedCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CmsComments_EntityType_EntityId",
                table: "CmsComments",
                columns: new[] { "EntityType", "EntityId" });
        }
    }
}
