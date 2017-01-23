using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.Abp.Identity.EntityFrameworkCore.Migrations
{
    public partial class Fix_IdentityUserRole_UserId_Nav_Property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRoles_IdentityUsers_IdentityUserId1",
                table: "IdentityUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUserRoles_IdentityUserId1",
                table: "IdentityUserRoles");

            migrationBuilder.DropColumn(
                name: "IdentityUserId1",
                table: "IdentityUserRoles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdentityUserId1",
                table: "IdentityUserRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRoles_IdentityUserId1",
                table: "IdentityUserRoles",
                column: "IdentityUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRoles_IdentityUsers_IdentityUserId1",
                table: "IdentityUserRoles",
                column: "IdentityUserId1",
                principalTable: "IdentityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
