using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Host.Migrations
{
    public partial class AddTenantfullaudited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LogoUri",
                table: "IdentityServerClients",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontChannelLogoutUri",
                table: "IdentityServerClients",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientUri",
                table: "IdentityServerClients",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackChannelLogoutUri",
                table: "IdentityServerClients",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "RedirectUri",
            //    table: "IdentityServerClientRedirectUris",
            //    maxLength: 200,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "IdentityServerClientProperties",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Key",
            //    table: "IdentityServerClientProperties",
            //    maxLength: 64,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 250);

            migrationBuilder.AddColumn<string>(
                name: "TenantName",
                table: "AbpAuditLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantName",
                table: "AbpAuditLogs");

            migrationBuilder.AlterColumn<string>(
                name: "LogoUri",
                table: "IdentityServerClients",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FrontChannelLogoutUri",
                table: "IdentityServerClients",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientUri",
                table: "IdentityServerClients",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BackChannelLogoutUri",
                table: "IdentityServerClients",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RedirectUri",
                table: "IdentityServerClientRedirectUris",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "IdentityServerClientProperties",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "IdentityServerClientProperties",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 64);
        }
    }
}
