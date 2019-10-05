using Microsoft.EntityFrameworkCore.Migrations;

namespace Acme.BookStore.Migrations
{
    public partial class Upgraded_To_Abp_0_21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "IdentityServerClients");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "IdentityServerIdentityResources",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeviceCodeLifetime",
                table: "IdentityServerClients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserCodeType",
                table: "IdentityServerClients",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserSsoLifetime",
                table: "IdentityServerClients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "IdentityServerApiResources",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "IdentityServerClients",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "IdentityServerClients");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "IdentityServerIdentityResources");

            migrationBuilder.DropColumn(
                name: "DeviceCodeLifetime",
                table: "IdentityServerClients");

            migrationBuilder.DropColumn(
                name: "UserCodeType",
                table: "IdentityServerClients");

            migrationBuilder.DropColumn(
                name: "UserSsoLifetime",
                table: "IdentityServerClients");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "IdentityServerApiResources");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "IdentityServerClients",
                column: "ClientId",
                unique: true);
        }
    }
}
