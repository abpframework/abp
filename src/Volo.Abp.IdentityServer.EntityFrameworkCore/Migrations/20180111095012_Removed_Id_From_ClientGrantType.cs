using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore.Migrations
{
    public partial class Removed_Id_From_ClientGrantType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientGrantTypes",
                table: "AbpIdsClientGrantTypes");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientGrantTypes_ClientId",
                table: "AbpIdsClientGrantTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientGrantTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientGrantTypes",
                table: "AbpIdsClientGrantTypes",
                columns: new[] { "ClientId", "GrantType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientGrantTypes",
                table: "AbpIdsClientGrantTypes");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientGrantTypes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientGrantTypes",
                table: "AbpIdsClientGrantTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientGrantTypes_ClientId",
                table: "AbpIdsClientGrantTypes",
                column: "ClientId");
        }
    }
}
