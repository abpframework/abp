using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Volo.Abp.IdentityServer.EntityFrameworkCore.Migrations
{
    public partial class IdentityServer_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpIdsApiResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AccessTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    AccessTokenType = table.Column<int>(type: "int", nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(type: "bit", nullable: false),
                    AllowOfflineAccess = table.Column<bool>(type: "bit", nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(type: "bit", nullable: false),
                    AllowRememberConsent = table.Column<bool>(type: "bit", nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "bit", nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "bit", nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(type: "int", nullable: false),
                    BackChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false),
                    BackChannelLogoutUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ClientClaimsPrefix = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ClientUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ConsentLifetime = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EnableLocalLogin = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IdentityTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    IncludeJwtId = table.Column<bool>(type: "bit", nullable: false),
                    LogoUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProtocolType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "int", nullable: false),
                    RefreshTokenUsage = table.Column<int>(type: "int", nullable: false),
                    RequireClientSecret = table.Column<bool>(type: "bit", nullable: false),
                    RequireConsent = table.Column<bool>(type: "bit", nullable: false),
                    RequirePkce = table.Column<bool>(type: "bit", nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(type: "int", nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsIdentityResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsIdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsPersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsPersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsApiClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsApiClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsApiClaims_AbpIdsApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "AbpIdsApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsApiScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Required = table.Column<bool>(type: "bit", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsApiScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsApiScopes_AbpIdsApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "AbpIdsApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsApiSecrets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsApiSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsApiSecrets_AbpIdsApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "AbpIdsApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientClaims_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientCorsOrigins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientCorsOrigins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientCorsOrigins_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientGrantTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrantType = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientGrantTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientGrantTypes_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientIdPRestrictions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientIdPRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientIdPRestrictions_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientPostLogoutRedirectUris",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostLogoutRedirectUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientPostLogoutRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientPostLogoutRedirectUris_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientProperties_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientRedirectUris",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RedirectUri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientRedirectUris_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientScopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientScopes_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsClientSecrets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsClientSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsClientSecrets_AbpIdsClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AbpIdsClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsIdentityClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsIdentityClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsIdentityClaims_AbpIdsIdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "AbpIdsIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpIdsApiScopeClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiScopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpIdsApiScopeClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpIdsApiScopeClaims_AbpIdsApiScopes_ApiScopeId",
                        column: x => x.ApiScopeId,
                        principalTable: "AbpIdsApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiClaims_ApiResourceId",
                table: "AbpIdsApiClaims",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiResources_Name",
                table: "AbpIdsApiResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsApiScopeClaims_ApiScopeId",
                table: "AbpIdsApiScopeClaims",
                column: "ApiScopeId");

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
                name: "IX_AbpIdsApiSecrets_ApiResourceId",
                table: "AbpIdsApiSecrets",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientClaims_ClientId",
                table: "AbpIdsClientClaims",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientCorsOrigins_ClientId",
                table: "AbpIdsClientCorsOrigins",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientGrantTypes_ClientId",
                table: "AbpIdsClientGrantTypes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientIdPRestrictions_ClientId",
                table: "AbpIdsClientIdPRestrictions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientPostLogoutRedirectUris_ClientId",
                table: "AbpIdsClientPostLogoutRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientProperties_ClientId",
                table: "AbpIdsClientProperties",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientRedirectUris_ClientId",
                table: "AbpIdsClientRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClients_ClientId",
                table: "AbpIdsClients",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientScopes_ClientId",
                table: "AbpIdsClientScopes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsClientSecrets_ClientId",
                table: "AbpIdsClientSecrets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsIdentityClaims_IdentityResourceId",
                table: "AbpIdsIdentityClaims",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsIdentityResources_Name",
                table: "AbpIdsIdentityResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpIdsPersistedGrants_SubjectId_ClientId_Type",
                table: "AbpIdsPersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpIdsApiClaims");

            migrationBuilder.DropTable(
                name: "AbpIdsApiScopeClaims");

            migrationBuilder.DropTable(
                name: "AbpIdsApiSecrets");

            migrationBuilder.DropTable(
                name: "AbpIdsClientClaims");

            migrationBuilder.DropTable(
                name: "AbpIdsClientCorsOrigins");

            migrationBuilder.DropTable(
                name: "AbpIdsClientGrantTypes");

            migrationBuilder.DropTable(
                name: "AbpIdsClientIdPRestrictions");

            migrationBuilder.DropTable(
                name: "AbpIdsClientPostLogoutRedirectUris");

            migrationBuilder.DropTable(
                name: "AbpIdsClientProperties");

            migrationBuilder.DropTable(
                name: "AbpIdsClientRedirectUris");

            migrationBuilder.DropTable(
                name: "AbpIdsClientScopes");

            migrationBuilder.DropTable(
                name: "AbpIdsClientSecrets");

            migrationBuilder.DropTable(
                name: "AbpIdsIdentityClaims");

            migrationBuilder.DropTable(
                name: "AbpIdsPersistedGrants");

            migrationBuilder.DropTable(
                name: "AbpIdsApiScopes");

            migrationBuilder.DropTable(
                name: "AbpIdsClients");

            migrationBuilder.DropTable(
                name: "AbpIdsIdentityResources");

            migrationBuilder.DropTable(
                name: "AbpIdsApiResources");
        }
    }
}
