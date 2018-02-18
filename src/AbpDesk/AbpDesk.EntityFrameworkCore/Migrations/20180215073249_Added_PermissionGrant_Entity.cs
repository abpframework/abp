using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AbpDesk.EntityFrameworkCore.Migrations
{
    public partial class Added_PermissionGrant_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MtTenants_Name",
                table: "MtTenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MtTenantConnectionStrings",
                table: "MtTenantConnectionStrings");

            migrationBuilder.DropIndex(
                name: "IX_MtTenantConnectionStrings_TenantId",
                table: "MtTenantConnectionStrings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MtTenantConnectionStrings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MtTenantConnectionStrings",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MtTenantConnectionStrings",
                table: "MtTenantConnectionStrings",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateTable(
                name: "AbpPermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: false),
                    ProviderName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MtTenants_Name",
                table: "MtTenants",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGrants_Name_ProviderName_ProviderKey",
                table: "AbpPermissionGrants",
                columns: new[] { "Name", "ProviderName", "ProviderKey" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpPermissionGrants");

            migrationBuilder.DropIndex(
                name: "IX_MtTenants_Name",
                table: "MtTenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MtTenantConnectionStrings",
                table: "MtTenantConnectionStrings");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MtTenantConnectionStrings",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MtTenantConnectionStrings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MtTenantConnectionStrings",
                table: "MtTenantConnectionStrings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MtTenants_Name",
                table: "MtTenants",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MtTenantConnectionStrings_TenantId",
                table: "MtTenantConnectionStrings",
                column: "TenantId");
        }
    }
}
