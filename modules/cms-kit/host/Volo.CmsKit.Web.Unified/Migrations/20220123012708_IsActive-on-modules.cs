using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volo.CmsKit.Migrations
{
    public partial class IsActiveonmodules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CmsUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "TenantName",
                table: "AbpAuditLogs",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImpersonatorTenantName",
                table: "AbpAuditLogs",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImpersonatorUserName",
                table: "AbpAuditLogs",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CmsUsers");

            migrationBuilder.DropColumn(
                name: "ImpersonatorTenantName",
                table: "AbpAuditLogs");

            migrationBuilder.DropColumn(
                name: "ImpersonatorUserName",
                table: "AbpAuditLogs");

            migrationBuilder.AlterColumn<string>(
                name: "TenantName",
                table: "AbpAuditLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
