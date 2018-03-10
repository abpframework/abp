using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Volo.Abp.Identity.EntityFrameworkCore.Migrations
{
    public partial class Made_Identity_Entities_MultiTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "IdentityUserTokens",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "IdentityUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "IdentityUserRoles",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "IdentityUserLogins",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "IdentityUserClaims",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "IdentityRoles",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "IdentityRoleClaims",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IdentityUserTokens");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IdentityUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IdentityUserRoles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IdentityUserLogins");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IdentityUserClaims");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IdentityRoles");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "IdentityRoleClaims");
        }
    }
}
