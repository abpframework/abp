using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerHost.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IdentityServerApiResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    ClientName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ClientUri = table.Column<string>(maxLength: 2000, nullable: true),
                    LogoUri = table.Column<string>(maxLength: 2000, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    ProtocolType = table.Column<string>(maxLength: 200, nullable: false),
                    RequireClientSecret = table.Column<bool>(nullable: false),
                    RequireConsent = table.Column<bool>(nullable: false),
                    AllowRememberConsent = table.Column<bool>(nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(nullable: false),
                    RequirePkce = table.Column<bool>(nullable: false),
                    AllowPlainTextPkce = table.Column<bool>(nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(nullable: false),
                    FrontChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    BackChannelLogoutUri = table.Column<string>(maxLength: 2000, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(nullable: false),
                    AllowOfflineAccess = table.Column<bool>(nullable: false),
                    IdentityTokenLifetime = table.Column<int>(nullable: false),
                    AccessTokenLifetime = table.Column<int>(nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>(nullable: false),
                    ConsentLifetime = table.Column<int>(nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>(nullable: false),
                    RefreshTokenUsage = table.Column<int>(nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(nullable: false),
                    RefreshTokenExpiration = table.Column<int>(nullable: false),
                    AccessTokenType = table.Column<int>(nullable: false),
                    EnableLocalLogin = table.Column<bool>(nullable: false),
                    IncludeJwtId = table.Column<bool>(nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>(nullable: false),
                    ClientClaimsPrefix = table.Column<string>(maxLength: 200, nullable: true),
                    PairWiseSubjectSalt = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerPersistedGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerPersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiClaims",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 196, nullable: false),
                    ApiResourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiClaims", x => new { x.ApiResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiClaims_IdentityServerApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopes",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 196, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    Required = table.Column<bool>(nullable: false),
                    Emphasize = table.Column<bool>(nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopes", x => new { x.ApiResourceId, x.Name });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiScopes_IdentityServerApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiSecrets",
                columns: table => new
                {
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 32, nullable: false),
                    Value = table.Column<string>(maxLength: 196, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    ApiResourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiSecrets", x => new { x.ApiResourceId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiSecrets_IdentityServerApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityServerClientClaims_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientCorsOrigins",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Origin = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientCorsOrigins", x => new { x.ClientId, x.Origin });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientCorsOrigins_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientGrantTypes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    GrantType = table.Column<string>(maxLength: 196, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientGrantTypes", x => new { x.ClientId, x.GrantType });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientGrantTypes_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientIdPRestrictions",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Provider = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientIdPRestrictions", x => new { x.ClientId, x.Provider });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientIdPRestrictions_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientPostLogoutRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    PostLogoutRedirectUri = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientPostLogoutRedirectUris", x => new { x.ClientId, x.PostLogoutRedirectUri });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientPostLogoutRedirectUris_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientProperties",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(maxLength: 250, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientProperties", x => new { x.ClientId, x.Key });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientProperties_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientRedirectUris",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    RedirectUri = table.Column<string>(maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientRedirectUris", x => new { x.ClientId, x.RedirectUri });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientRedirectUris_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientScopes",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    Scope = table.Column<string>(maxLength: 196, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientScopes", x => new { x.ClientId, x.Scope });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientScopes_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerClientSecrets",
                columns: table => new
                {
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 32, nullable: false),
                    Value = table.Column<string>(maxLength: 196, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    ClientId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerClientSecrets", x => new { x.ClientId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerClientSecrets_IdentityServerClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "IdentityServerClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityClaims",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 196, nullable: false),
                    IdentityResourceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityClaims", x => new { x.IdentityResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityClaims_IdentityServerIdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopeClaims",
                columns: table => new
                {
                    Type = table.Column<string>(maxLength: 196, nullable: false),
                    ApiResourceId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 196, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopeClaims", x => new { x.ApiResourceId, x.Name, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiResourceId_Name",
                        columns: x => new { x.ApiResourceId, x.Name },
                        principalTable: "IdentityServerApiScopes",
                        principalColumns: new[] { "ApiResourceId", "Name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerClientClaims_ClientId",
                table: "IdentityServerClientClaims",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerClients_ClientId",
                table: "IdentityServerClients",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_ClientId_Type",
                table: "IdentityServerPersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityServerApiClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopeClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerApiSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerClientClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerClientCorsOrigins");

            migrationBuilder.DropTable(
                name: "IdentityServerClientGrantTypes");

            migrationBuilder.DropTable(
                name: "IdentityServerClientIdPRestrictions");

            migrationBuilder.DropTable(
                name: "IdentityServerClientPostLogoutRedirectUris");

            migrationBuilder.DropTable(
                name: "IdentityServerClientProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerClientRedirectUris");

            migrationBuilder.DropTable(
                name: "IdentityServerClientScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerClientSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerPersistedGrants");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerClients");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResources");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResources");
        }
    }
}
