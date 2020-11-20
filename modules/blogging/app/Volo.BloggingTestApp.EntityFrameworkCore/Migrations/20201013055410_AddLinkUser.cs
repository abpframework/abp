using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.BloggingTestApp.EntityFrameworkCore.Migrations
{
    public partial class AddLinkUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpLinkUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SourceUserId = table.Column<Guid>(nullable: false),
                    SourceTenantId = table.Column<Guid>(nullable: true),
                    TargetUserId = table.Column<Guid>(nullable: false),
                    TargetTenantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLinkUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId",
                table: "AbpLinkUsers",
                columns: new[] { "SourceUserId", "SourceTenantId", "TargetUserId", "TargetTenantId" },
                unique: true,
                filter: "[SourceTenantId] IS NOT NULL AND [TargetTenantId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpLinkUsers");
        }
    }
}
