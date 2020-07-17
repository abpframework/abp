﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.CmsKit.Migrations
{
    public partial class Added_UserReactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CmsUserReactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EntityType = table.Column<string>(maxLength: 64, nullable: false),
                    EntityId = table.Column<string>(maxLength: 64, nullable: false),
                    ReactionName = table.Column<string>(maxLength: 32, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsUserReactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserReactions_EntityType_EntityId",
                table: "CmsUserReactions",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CmsUserReactions_UserId_EntityType_EntityId_ReactionName",
                table: "CmsUserReactions",
                columns: new[] { "UserId", "EntityType", "EntityId", "ReactionName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CmsUserReactions");
        }
    }
}
