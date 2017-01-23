using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.Abp.Identity.EntityFrameworkCore.Migrations
{
    public partial class Remove_Unique_Index_Constraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityUserTokens_UserId_LoginProvider_Name",
                table: "IdentityUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUserRoles_UserId_RoleId",
                table: "IdentityUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUserLogins_UserId_LoginProvider_ProviderKey",
                table: "IdentityUserLogins");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "IdentityUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "IdentityRoles");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserTokens_UserId_LoginProvider_Name",
                table: "IdentityUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRoles_UserId_RoleId",
                table: "IdentityUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserLogins_UserId_LoginProvider_ProviderKey",
                table: "IdentityUserLogins",
                columns: new[] { "UserId", "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "IdentityUsers",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "IdentityRoles",
                column: "NormalizedName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityUserTokens_UserId_LoginProvider_Name",
                table: "IdentityUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUserRoles_UserId_RoleId",
                table: "IdentityUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUserLogins_UserId_LoginProvider_ProviderKey",
                table: "IdentityUserLogins");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "IdentityUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "IdentityRoles");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserTokens_UserId_LoginProvider_Name",
                table: "IdentityUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRoles_UserId_RoleId",
                table: "IdentityUserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserLogins_UserId_LoginProvider_ProviderKey",
                table: "IdentityUserLogins",
                columns: new[] { "UserId", "LoginProvider", "ProviderKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "IdentityUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "IdentityRoles",
                column: "NormalizedName",
                unique: true);
        }
    }
}
