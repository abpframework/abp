using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AbpDesk.EntityFrameworkCore.Migrations
{
    public partial class MultiTenancyModuleAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MtTenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MtTenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MtTenantConnectionStrings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MtTenantConnectionStrings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MtTenantConnectionStrings_MtTenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "MtTenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MtTenantConnectionStrings_TenantId",
                table: "MtTenantConnectionStrings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MtTenants_Name",
                table: "MtTenants",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MtTenantConnectionStrings");

            migrationBuilder.DropTable(
                name: "MtTenants");
        }
    }
}
