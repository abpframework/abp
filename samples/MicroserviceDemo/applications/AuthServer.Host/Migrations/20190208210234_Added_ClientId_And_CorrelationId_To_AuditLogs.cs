using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Host.Migrations
{
    public partial class Added_ClientId_And_CorrelationId_To_AuditLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                "PK_IdentityServerClientPostLogoutRedirectUris",
                "IdentityServerClientPostLogoutRedirectUris"
            );

            migrationBuilder.AlterColumn<string>(
                name: "PostLogoutRedirectUri",
                table: "IdentityServerClientPostLogoutRedirectUris",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            migrationBuilder.AddPrimaryKey(
                "PK_IdentityServerClientPostLogoutRedirectUris",
                "IdentityServerClientPostLogoutRedirectUris",
                new[] {"ClientId", "PostLogoutRedirectUri"}
            );

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "AbpAuditLogs",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "AbpAuditLogs",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AbpAuditLogs");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "AbpAuditLogs");

            migrationBuilder.DropPrimaryKey(
                "PK_IdentityServerClientPostLogoutRedirectUris",
                "IdentityServerClientPostLogoutRedirectUris"
            );

            migrationBuilder.AlterColumn<string>(
                name: "PostLogoutRedirectUri",
                table: "IdentityServerClientPostLogoutRedirectUris",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                "PK_IdentityServerClientPostLogoutRedirectUris",
                "IdentityServerClientPostLogoutRedirectUris",
                new[] { "ClientId", "PostLogoutRedirectUri" }
            );
        }
    }
}
