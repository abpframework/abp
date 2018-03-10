using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Volo.Abp.EntityFrameworkCore.Tests.Migrations
{
    public partial class Added_TenantId_To_Person : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "People",
                type: "BLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "People");
        }
    }
}
