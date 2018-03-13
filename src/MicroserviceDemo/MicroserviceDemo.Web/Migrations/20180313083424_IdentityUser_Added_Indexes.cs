using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MicroserviceDemo.Web.Migrations
{
    public partial class IdentityUser_Added_Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IdentityUsers_Email",
                table: "IdentityUsers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUsers_UserName",
                table: "IdentityUsers",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityUsers_Email",
                table: "IdentityUsers");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUsers_UserName",
                table: "IdentityUsers");
        }
    }
}
