using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore.Migrations
{
    public partial class IdentityServer_Revisions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsApiClaims_AbpIdsApiResources_ApiResourceId",
                table: "AbpIdsApiClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsApiScopeClaims_AbpIdsApiScopes_ApiScopeId",
                table: "AbpIdsApiScopeClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsApiScopes_AbpIdsApiResources_ApiResourceId",
                table: "AbpIdsApiScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsApiSecrets_AbpIdsApiResources_ApiResourceId",
                table: "AbpIdsApiSecrets");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientClaims_AbpIdsClients_ClientId",
                table: "AbpIdsClientClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientCorsOrigins_AbpIdsClients_ClientId",
                table: "AbpIdsClientCorsOrigins");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientGrantTypes_AbpIdsClients_ClientId",
                table: "AbpIdsClientGrantTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientIdPRestrictions_AbpIdsClients_ClientId",
                table: "AbpIdsClientIdPRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientPostLogoutRedirectUris_AbpIdsClients_ClientId",
                table: "AbpIdsClientPostLogoutRedirectUris");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientProperties_AbpIdsClients_ClientId",
                table: "AbpIdsClientProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientRedirectUris_AbpIdsClients_ClientId",
                table: "AbpIdsClientRedirectUris");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientScopes_AbpIdsClients_ClientId",
                table: "AbpIdsClientScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsClientSecrets_AbpIdsClients_ClientId",
                table: "AbpIdsClientSecrets");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpIdsIdentityClaims_AbpIdsIdentityResources_IdentityResourceId",
                table: "AbpIdsIdentityClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsPersistedGrants",
                table: "AbpIdsPersistedGrants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsIdentityResources",
                table: "AbpIdsIdentityResources");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsIdentityResources_Name",
                table: "AbpIdsIdentityResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsIdentityClaims",
                table: "AbpIdsIdentityClaims");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsIdentityClaims_IdentityResourceId",
                table: "AbpIdsIdentityClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientSecrets",
                table: "AbpIdsClientSecrets");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientSecrets_ClientId",
                table: "AbpIdsClientSecrets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientScopes",
                table: "AbpIdsClientScopes");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientScopes_ClientId",
                table: "AbpIdsClientScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClients",
                table: "AbpIdsClients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientRedirectUris",
                table: "AbpIdsClientRedirectUris");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientRedirectUris_ClientId",
                table: "AbpIdsClientRedirectUris");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientProperties",
                table: "AbpIdsClientProperties");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientProperties_ClientId",
                table: "AbpIdsClientProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientPostLogoutRedirectUris",
                table: "AbpIdsClientPostLogoutRedirectUris");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientPostLogoutRedirectUris_ClientId",
                table: "AbpIdsClientPostLogoutRedirectUris");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientIdPRestrictions",
                table: "AbpIdsClientIdPRestrictions");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientIdPRestrictions_ClientId",
                table: "AbpIdsClientIdPRestrictions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientGrantTypes",
                table: "AbpIdsClientGrantTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientCorsOrigins",
                table: "AbpIdsClientCorsOrigins");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsClientCorsOrigins_ClientId",
                table: "AbpIdsClientCorsOrigins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsClientClaims",
                table: "AbpIdsClientClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsApiSecrets",
                table: "AbpIdsApiSecrets");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsApiSecrets_ApiResourceId",
                table: "AbpIdsApiSecrets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsApiScopes",
                table: "AbpIdsApiScopes");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsApiScopes_ApiResourceId",
                table: "AbpIdsApiScopes");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsApiScopes_Name",
                table: "AbpIdsApiScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsApiScopeClaims",
                table: "AbpIdsApiScopeClaims");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsApiScopeClaims_ApiScopeId",
                table: "AbpIdsApiScopeClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsApiResources",
                table: "AbpIdsApiResources");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsApiResources_Name",
                table: "AbpIdsApiResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpIdsApiClaims",
                table: "AbpIdsApiClaims");

            migrationBuilder.DropIndex(
                name: "IX_AbpIdsApiClaims_ApiResourceId",
                table: "AbpIdsApiClaims");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsIdentityClaims");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientSecrets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientScopes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientRedirectUris");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientProperties");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientPostLogoutRedirectUris");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientIdPRestrictions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsClientCorsOrigins");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsApiSecrets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsApiScopes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsApiScopeClaims");

            migrationBuilder.DropColumn(
                name: "ApiScopeId",
                table: "AbpIdsApiScopeClaims");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AbpIdsApiClaims");

            migrationBuilder.RenameTable(
                name: "AbpIdsPersistedGrants",
                newName: "IdentityServerPersistedGrants");

            migrationBuilder.RenameTable(
                name: "AbpIdsIdentityResources",
                newName: "IdentityServerIdentityResources");

            migrationBuilder.RenameTable(
                name: "AbpIdsIdentityClaims",
                newName: "IdentityServerIdentityClaims");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientSecrets",
                newName: "IdentityServerClientSecrets");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientScopes",
                newName: "IdentityServerClientScopes");

            migrationBuilder.RenameTable(
                name: "AbpIdsClients",
                newName: "IdentityServerClients");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientRedirectUris",
                newName: "IdentityServerClientRedirectUris");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientProperties",
                newName: "IdentityServerClientProperties");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientPostLogoutRedirectUris",
                newName: "IdentityServerClientPostLogoutRedirectUris");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientIdPRestrictions",
                newName: "IdentityServerClientIdPRestrictions");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientGrantTypes",
                newName: "IdentityServerClientGrantTypes");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientCorsOrigins",
                newName: "IdentityServerClientCorsOrigins");

            migrationBuilder.RenameTable(
                name: "AbpIdsClientClaims",
                newName: "IdentityServerClientClaims");

            migrationBuilder.RenameTable(
                name: "AbpIdsApiSecrets",
                newName: "IdentityServerApiSecrets");

            migrationBuilder.RenameTable(
                name: "AbpIdsApiScopes",
                newName: "IdentityServerApiScopes");

            migrationBuilder.RenameTable(
                name: "AbpIdsApiScopeClaims",
                newName: "IdentityServerApiScopeClaims");

            migrationBuilder.RenameTable(
                name: "AbpIdsApiResources",
                newName: "IdentityServerApiResources");

            migrationBuilder.RenameTable(
                name: "AbpIdsApiClaims",
                newName: "IdentityServerApiClaims");

            migrationBuilder.RenameIndex(
                name: "IX_AbpIdsPersistedGrants_SubjectId_ClientId_Type",
                table: "IdentityServerPersistedGrants",
                newName: "IX_IdentityServerPersistedGrants_SubjectId_ClientId_Type");

            migrationBuilder.RenameIndex(
                name: "IX_AbpIdsClients_ClientId",
                table: "IdentityServerClients",
                newName: "IX_IdentityServerClients_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpIdsClientClaims_ClientId",
                table: "IdentityServerClientClaims",
                newName: "IX_IdentityServerClientClaims_ClientId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "IdentityServerIdentityClaims",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "IdentityServerClientSecrets",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "IdentityServerClientSecrets",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "IdentityServerClientSecrets",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "IdentityServerClientScopes",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "IdentityServerClientIdPRestrictions",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "GrantType",
                table: "IdentityServerClientGrantTypes",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "IdentityServerApiSecrets",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "IdentityServerApiSecrets",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "IdentityServerApiSecrets",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IdentityServerApiScopes",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "IdentityServerApiScopes",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "IdentityServerApiScopes",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "IdentityServerApiScopeClaims",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddColumn<Guid>(
                name: "ApiResourceId",
                table: "IdentityServerApiScopeClaims",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "IdentityServerApiScopeClaims",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "IdentityServerApiClaims",
                type: "nvarchar(196)",
                maxLength: 196,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerPersistedGrants",
                table: "IdentityServerPersistedGrants",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerIdentityResources",
                table: "IdentityServerIdentityResources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerIdentityClaims",
                table: "IdentityServerIdentityClaims",
                columns: new[] { "IdentityResourceId", "Type" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientSecrets",
                table: "IdentityServerClientSecrets",
                columns: new[] { "ClientId", "Type", "Value" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientScopes",
                table: "IdentityServerClientScopes",
                columns: new[] { "ClientId", "Scope" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClients",
                table: "IdentityServerClients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientRedirectUris",
                table: "IdentityServerClientRedirectUris",
                columns: new[] { "ClientId", "RedirectUri" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientProperties",
                table: "IdentityServerClientProperties",
                columns: new[] { "ClientId", "Key" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientPostLogoutRedirectUris",
                table: "IdentityServerClientPostLogoutRedirectUris",
                columns: new[] { "ClientId", "PostLogoutRedirectUri" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientIdPRestrictions",
                table: "IdentityServerClientIdPRestrictions",
                columns: new[] { "ClientId", "Provider" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientGrantTypes",
                table: "IdentityServerClientGrantTypes",
                columns: new[] { "ClientId", "GrantType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientCorsOrigins",
                table: "IdentityServerClientCorsOrigins",
                columns: new[] { "ClientId", "Origin" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientClaims",
                table: "IdentityServerClientClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiSecrets",
                table: "IdentityServerApiSecrets",
                columns: new[] { "ApiResourceId", "Type", "Value" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiScopes",
                table: "IdentityServerApiScopes",
                columns: new[] { "ApiResourceId", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiScopeClaims",
                table: "IdentityServerApiScopeClaims",
                columns: new[] { "ApiResourceId", "Name", "Type" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiResources",
                table: "IdentityServerApiResources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiClaims",
                table: "IdentityServerApiClaims",
                columns: new[] { "ApiResourceId", "Type" });

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiClaims_IdentityServerApiResources_ApiResourceId",
                table: "IdentityServerApiClaims",
                column: "ApiResourceId",
                principalTable: "IdentityServerApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiResourceId_Name",
                table: "IdentityServerApiScopeClaims",
                columns: new[] { "ApiResourceId", "Name" },
                principalTable: "IdentityServerApiScopes",
                principalColumns: new[] { "ApiResourceId", "Name" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiScopes_IdentityServerApiResources_ApiResourceId",
                table: "IdentityServerApiScopes",
                column: "ApiResourceId",
                principalTable: "IdentityServerApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiSecrets_IdentityServerApiResources_ApiResourceId",
                table: "IdentityServerApiSecrets",
                column: "ApiResourceId",
                principalTable: "IdentityServerApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientClaims_IdentityServerClients_ClientId",
                table: "IdentityServerClientClaims",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientCorsOrigins_IdentityServerClients_ClientId",
                table: "IdentityServerClientCorsOrigins",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientGrantTypes_IdentityServerClients_ClientId",
                table: "IdentityServerClientGrantTypes",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientIdPRestrictions_IdentityServerClients_ClientId",
                table: "IdentityServerClientIdPRestrictions",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientPostLogoutRedirectUris_IdentityServerClients_ClientId",
                table: "IdentityServerClientPostLogoutRedirectUris",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientProperties_IdentityServerClients_ClientId",
                table: "IdentityServerClientProperties",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientRedirectUris_IdentityServerClients_ClientId",
                table: "IdentityServerClientRedirectUris",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientScopes_IdentityServerClients_ClientId",
                table: "IdentityServerClientScopes",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerClientSecrets_IdentityServerClients_ClientId",
                table: "IdentityServerClientSecrets",
                column: "ClientId",
                principalTable: "IdentityServerClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerIdentityClaims_IdentityServerIdentityResources_IdentityResourceId",
                table: "IdentityServerIdentityClaims",
                column: "IdentityResourceId",
                principalTable: "IdentityServerIdentityResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiClaims_IdentityServerApiResources_ApiResourceId",
                table: "IdentityServerApiClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiResourceId_Name",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiScopes_IdentityServerApiResources_ApiResourceId",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiSecrets_IdentityServerApiResources_ApiResourceId",
                table: "IdentityServerApiSecrets");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientClaims_IdentityServerClients_ClientId",
                table: "IdentityServerClientClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientCorsOrigins_IdentityServerClients_ClientId",
                table: "IdentityServerClientCorsOrigins");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientGrantTypes_IdentityServerClients_ClientId",
                table: "IdentityServerClientGrantTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientIdPRestrictions_IdentityServerClients_ClientId",
                table: "IdentityServerClientIdPRestrictions");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientPostLogoutRedirectUris_IdentityServerClients_ClientId",
                table: "IdentityServerClientPostLogoutRedirectUris");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientProperties_IdentityServerClients_ClientId",
                table: "IdentityServerClientProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientRedirectUris_IdentityServerClients_ClientId",
                table: "IdentityServerClientRedirectUris");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientScopes_IdentityServerClients_ClientId",
                table: "IdentityServerClientScopes");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerClientSecrets_IdentityServerClients_ClientId",
                table: "IdentityServerClientSecrets");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerIdentityClaims_IdentityServerIdentityResources_IdentityResourceId",
                table: "IdentityServerIdentityClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerPersistedGrants",
                table: "IdentityServerPersistedGrants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerIdentityResources",
                table: "IdentityServerIdentityResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerIdentityClaims",
                table: "IdentityServerIdentityClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientSecrets",
                table: "IdentityServerClientSecrets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientScopes",
                table: "IdentityServerClientScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClients",
                table: "IdentityServerClients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientRedirectUris",
                table: "IdentityServerClientRedirectUris");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientProperties",
                table: "IdentityServerClientProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientPostLogoutRedirectUris",
                table: "IdentityServerClientPostLogoutRedirectUris");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientIdPRestrictions",
                table: "IdentityServerClientIdPRestrictions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientGrantTypes",
                table: "IdentityServerClientGrantTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientCorsOrigins",
                table: "IdentityServerClientCorsOrigins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientClaims",
                table: "IdentityServerClientClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiSecrets",
                table: "IdentityServerApiSecrets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiScopes",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiScopeClaims",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiResources",
                table: "IdentityServerApiResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiClaims",
                table: "IdentityServerApiClaims");

            migrationBuilder.DropColumn(
                name: "ApiResourceId",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.RenameTable(
                name: "IdentityServerPersistedGrants",
                newName: "AbpIdsPersistedGrants");

            migrationBuilder.RenameTable(
                name: "IdentityServerIdentityResources",
                newName: "AbpIdsIdentityResources");

            migrationBuilder.RenameTable(
                name: "IdentityServerIdentityClaims",
                newName: "AbpIdsIdentityClaims");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientSecrets",
                newName: "AbpIdsClientSecrets");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientScopes",
                newName: "AbpIdsClientScopes");

            migrationBuilder.RenameTable(
                name: "IdentityServerClients",
                newName: "AbpIdsClients");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientRedirectUris",
                newName: "AbpIdsClientRedirectUris");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientProperties",
                newName: "AbpIdsClientProperties");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientPostLogoutRedirectUris",
                newName: "AbpIdsClientPostLogoutRedirectUris");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientIdPRestrictions",
                newName: "AbpIdsClientIdPRestrictions");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientGrantTypes",
                newName: "AbpIdsClientGrantTypes");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientCorsOrigins",
                newName: "AbpIdsClientCorsOrigins");

            migrationBuilder.RenameTable(
                name: "IdentityServerClientClaims",
                newName: "AbpIdsClientClaims");

            migrationBuilder.RenameTable(
                name: "IdentityServerApiSecrets",
                newName: "AbpIdsApiSecrets");

            migrationBuilder.RenameTable(
                name: "IdentityServerApiScopes",
                newName: "AbpIdsApiScopes");

            migrationBuilder.RenameTable(
                name: "IdentityServerApiScopeClaims",
                newName: "AbpIdsApiScopeClaims");

            migrationBuilder.RenameTable(
                name: "IdentityServerApiResources",
                newName: "AbpIdsApiResources");

            migrationBuilder.RenameTable(
                name: "IdentityServerApiClaims",
                newName: "AbpIdsApiClaims");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_ClientId_Type",
                table: "AbpIdsPersistedGrants",
                newName: "IX_AbpIdsPersistedGrants_SubjectId_ClientId_Type");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "AbpIdsClients",
                newName: "IX_AbpIdsClients_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_IdentityServerClientClaims_ClientId",
                table: "AbpIdsClientClaims",
                newName: "IX_AbpIdsClientClaims_ClientId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AbpIdsIdentityClaims",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsIdentityClaims",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AbpIdsClientSecrets",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AbpIdsClientSecrets",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AbpIdsClientSecrets",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientSecrets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "AbpIdsClientScopes",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientScopes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientRedirectUris",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientProperties",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientPostLogoutRedirectUris",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Provider",
                table: "AbpIdsClientIdPRestrictions",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientIdPRestrictions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "GrantType",
                table: "AbpIdsClientGrantTypes",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsClientCorsOrigins",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AbpIdsApiSecrets",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AbpIdsApiSecrets",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AbpIdsApiSecrets",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsApiSecrets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "AbpIdsApiScopes",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AbpIdsApiScopes",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AbpIdsApiScopes",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsApiScopes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AbpIdsApiScopeClaims",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsApiScopeClaims",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ApiScopeId",
                table: "AbpIdsApiScopeClaims",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AbpIdsApiClaims",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(196)",
                oldMaxLength: 196);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AbpIdsApiClaims",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsPersistedGrants",
                table: "AbpIdsPersistedGrants",
                column: "Key");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsIdentityResources",
                table: "AbpIdsIdentityResources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsIdentityClaims",
                table: "AbpIdsIdentityClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientSecrets",
                table: "AbpIdsClientSecrets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientScopes",
                table: "AbpIdsClientScopes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClients",
                table: "AbpIdsClients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientRedirectUris",
                table: "AbpIdsClientRedirectUris",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientProperties",
                table: "AbpIdsClientProperties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientPostLogoutRedirectUris",
                table: "AbpIdsClientPostLogoutRedirectUris",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientIdPRestrictions",
                table: "AbpIdsClientIdPRestrictions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientGrantTypes",
                table: "AbpIdsClientGrantTypes",
                columns: new[] { "ClientId", "GrantType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientCorsOrigins",
                table: "AbpIdsClientCorsOrigins",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsClientClaims",
                table: "AbpIdsClientClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsApiSecrets",
                table: "AbpIdsApiSecrets",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsApiScopes",
                table: "AbpIdsApiScopes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsApiScopeClaims",
                table: "AbpIdsApiScopeClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsApiResources",
                table: "AbpIdsApiResources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpIdsApiClaims",
                table: "AbpIdsApiClaims",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsIdentityResources_Name",
                table: "AbpIdsIdentityResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsIdentityClaims_IdentityResourceId",
                table: "AbpIdsIdentityClaims",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientSecrets_ClientId",
                table: "AbpIdsClientSecrets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientScopes_ClientId",
                table: "AbpIdsClientScopes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientRedirectUris_ClientId",
                table: "AbpIdsClientRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientProperties_ClientId",
                table: "AbpIdsClientProperties",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientPostLogoutRedirectUris_ClientId",
                table: "AbpIdsClientPostLogoutRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientIdPRestrictions_ClientId",
                table: "AbpIdsClientIdPRestrictions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientCorsOrigins_ClientId",
                table: "AbpIdsClientCorsOrigins",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiSecrets_ApiResourceId",
                table: "AbpIdsApiSecrets",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiScopes_ApiResourceId",
                table: "AbpIdsApiScopes",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiScopes_Name",
                table: "AbpIdsApiScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiScopeClaims_ApiScopeId",
                table: "AbpIdsApiScopeClaims",
                column: "ApiScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiResources_Name",
                table: "AbpIdsApiResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiClaims_ApiResourceId",
                table: "AbpIdsApiClaims",
                column: "ApiResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsApiClaims_AbpIdsApiResources_ApiResourceId",
                table: "AbpIdsApiClaims",
                column: "ApiResourceId",
                principalTable: "AbpIdsApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsApiScopeClaims_AbpIdsApiScopes_ApiScopeId",
                table: "AbpIdsApiScopeClaims",
                column: "ApiScopeId",
                principalTable: "AbpIdsApiScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsApiScopes_AbpIdsApiResources_ApiResourceId",
                table: "AbpIdsApiScopes",
                column: "ApiResourceId",
                principalTable: "AbpIdsApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsApiSecrets_AbpIdsApiResources_ApiResourceId",
                table: "AbpIdsApiSecrets",
                column: "ApiResourceId",
                principalTable: "AbpIdsApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientClaims_AbpIdsClients_ClientId",
                table: "AbpIdsClientClaims",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientCorsOrigins_AbpIdsClients_ClientId",
                table: "AbpIdsClientCorsOrigins",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientGrantTypes_AbpIdsClients_ClientId",
                table: "AbpIdsClientGrantTypes",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientIdPRestrictions_AbpIdsClients_ClientId",
                table: "AbpIdsClientIdPRestrictions",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientPostLogoutRedirectUris_AbpIdsClients_ClientId",
                table: "AbpIdsClientPostLogoutRedirectUris",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientProperties_AbpIdsClients_ClientId",
                table: "AbpIdsClientProperties",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientRedirectUris_AbpIdsClients_ClientId",
                table: "AbpIdsClientRedirectUris",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientScopes_AbpIdsClients_ClientId",
                table: "AbpIdsClientScopes",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsClientSecrets_AbpIdsClients_ClientId",
                table: "AbpIdsClientSecrets",
                column: "ClientId",
                principalTable: "AbpIdsClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpIdsIdentityClaims_AbpIdsIdentityResources_IdentityResourceId",
                table: "AbpIdsIdentityClaims",
                column: "IdentityResourceId",
                principalTable: "AbpIdsIdentityResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
