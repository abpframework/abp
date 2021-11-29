using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.CmsKit.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AbpBlobContainers",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpBlobContainers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsBlogFeatures",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FeatureName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsBlogFeatures", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsBlogs",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Slug = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsBlogs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsComments",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EntityType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                EntityId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Text = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                RepliedCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsComments", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsEntityTags",
            columns: table => new {
                TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                EntityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsEntityTags", x => new { x.EntityId, x.TagId });
            });

        migrationBuilder.CreateTable(
            name: "CmsMediaDescriptors",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EntityType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                MimeType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                Size = table.Column<long>(type: "bigint", maxLength: 2147483647, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsMediaDescriptors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsMenus",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsMenus", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsPages",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Slug = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                Script = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsPages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsRatings",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EntityType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                EntityId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                StarCount = table.Column<short>(type: "smallint", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsRatings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsTags",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EntityType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsTags", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsUserReactions",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                EntityType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                EntityId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                ReactionName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsUserReactions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CmsUsers",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                Surname = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsUsers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AbpBlobs",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ContainerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                Content = table.Column<byte[]>(type: "varbinary(max)", maxLength: 2147483647, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AbpBlobs", x => x.Id);
                table.ForeignKey(
                    name: "FK_AbpBlobs_AbpBlobContainers_ContainerId",
                    column: x => x.ContainerId,
                    principalTable: "AbpBlobContainers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CmsMenuItems",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                MenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                Url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Order = table.Column<int>(type: "int", nullable: false),
                Target = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ElementId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CssClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RequiredPermissionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsMenuItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_CmsMenuItems_CmsMenus_MenuId",
                    column: x => x.MenuId,
                    principalTable: "CmsMenus",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CmsBlogPosts",
            columns: table => new {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                Slug = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                ShortDescription = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Content = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: true),
                CoverImageMediaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CmsBlogPosts", x => x.Id);
                table.ForeignKey(
                    name: "FK_CmsBlogPosts_CmsUsers_AuthorId",
                    column: x => x.AuthorId,
                    principalTable: "CmsUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AbpBlobContainers_TenantId_Name",
            table: "AbpBlobContainers",
            columns: new[] { "TenantId", "Name" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpBlobs_ContainerId",
            table: "AbpBlobs",
            column: "ContainerId");

        migrationBuilder.CreateIndex(
            name: "IX_AbpBlobs_TenantId_ContainerId_Name",
            table: "AbpBlobs",
            columns: new[] { "TenantId", "ContainerId", "Name" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsBlogPosts_AuthorId",
            table: "CmsBlogPosts",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_CmsBlogPosts_Slug_BlogId",
            table: "CmsBlogPosts",
            columns: new[] { "Slug", "BlogId" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsComments_TenantId_EntityType_EntityId",
            table: "CmsComments",
            columns: new[] { "TenantId", "EntityType", "EntityId" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsComments_TenantId_RepliedCommentId",
            table: "CmsComments",
            columns: new[] { "TenantId", "RepliedCommentId" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsEntityTags_TenantId_EntityId_TagId",
            table: "CmsEntityTags",
            columns: new[] { "TenantId", "EntityId", "TagId" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsMenuItems_MenuId",
            table: "CmsMenuItems",
            column: "MenuId");

        migrationBuilder.CreateIndex(
            name: "IX_CmsPages_TenantId_Slug",
            table: "CmsPages",
            columns: new[] { "TenantId", "Slug" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsRatings_TenantId_EntityType_EntityId_CreatorId",
            table: "CmsRatings",
            columns: new[] { "TenantId", "EntityType", "EntityId", "CreatorId" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsTags_TenantId_Name",
            table: "CmsTags",
            columns: new[] { "TenantId", "Name" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsUserReactions_TenantId_CreatorId_EntityType_EntityId_ReactionName",
            table: "CmsUserReactions",
            columns: new[] { "TenantId", "CreatorId", "EntityType", "EntityId", "ReactionName" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsUserReactions_TenantId_EntityType_EntityId_ReactionName",
            table: "CmsUserReactions",
            columns: new[] { "TenantId", "EntityType", "EntityId", "ReactionName" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsUsers_TenantId_Email",
            table: "CmsUsers",
            columns: new[] { "TenantId", "Email" });

        migrationBuilder.CreateIndex(
            name: "IX_CmsUsers_TenantId_UserName",
            table: "CmsUsers",
            columns: new[] { "TenantId", "UserName" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AbpBlobs");

        migrationBuilder.DropTable(
            name: "CmsBlogFeatures");

        migrationBuilder.DropTable(
            name: "CmsBlogPosts");

        migrationBuilder.DropTable(
            name: "CmsBlogs");

        migrationBuilder.DropTable(
            name: "CmsComments");

        migrationBuilder.DropTable(
            name: "CmsEntityTags");

        migrationBuilder.DropTable(
            name: "CmsMediaDescriptors");

        migrationBuilder.DropTable(
            name: "CmsMenuItems");

        migrationBuilder.DropTable(
            name: "CmsPages");

        migrationBuilder.DropTable(
            name: "CmsRatings");

        migrationBuilder.DropTable(
            name: "CmsTags");

        migrationBuilder.DropTable(
            name: "CmsUserReactions");

        migrationBuilder.DropTable(
            name: "AbpBlobContainers");

        migrationBuilder.DropTable(
            name: "CmsUsers");

        migrationBuilder.DropTable(
            name: "CmsMenus");
    }
}
