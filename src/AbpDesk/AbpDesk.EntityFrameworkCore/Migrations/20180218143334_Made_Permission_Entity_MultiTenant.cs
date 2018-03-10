using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AbpDesk.EntityFrameworkCore.Migrations
{
    public partial class Made_Permission_Entity_MultiTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AbpPermissionGrants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AbpPermissionGrants");
        }
    }
}
