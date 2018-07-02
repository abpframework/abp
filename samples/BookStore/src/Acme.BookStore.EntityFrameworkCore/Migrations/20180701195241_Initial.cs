using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Acme.BookStore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpPermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(maxLength: 64, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2048, nullable: false),
                    ProviderName = table.Column<string>(maxLength: 64, nullable: true),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: true),
                    SecurityStamp = table.Column<string>(maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 256, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false, defaultValue: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false, defaultValue: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false, defaultValue: false),
                    AccessFailedCount = table.Column<int>(nullable: false, defaultValue: 0),
                    ExtraProperties = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpRoleClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 1024, nullable: true),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: false),
                    ClaimValue = table.Column<string>(maxLength: 1024, nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserClaims_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserLogins",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 64, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 196, nullable: false),
                    ProviderDisplayName = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserLogins", x => new { x.UserId, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_AbpUserLogins_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserRoles",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpUserRoles_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserTokens",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AbpUserTokens_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGrants_Name_ProviderName_ProviderKey",
                table: "AbpPermissionGrants",
                columns: new[] { "Name", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoleClaims_RoleId",
                table: "AbpRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpRoles_NormalizedName",
                table: "AbpRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
                table: "AbpSettings",
                columns: new[] { "Name", "ProviderName", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserClaims_UserId",
                table: "AbpUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserLogins_LoginProvider_ProviderKey",
                table: "AbpUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserRoles_RoleId_UserId",
                table: "AbpUserRoles",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_Email",
                table: "AbpUsers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_NormalizedEmail",
                table: "AbpUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_NormalizedUserName",
                table: "AbpUsers",
                column: "NormalizedUserName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_UserName",
                table: "AbpUsers",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpPermissionGrants");

            migrationBuilder.DropTable(
                name: "AbpRoleClaims");

            migrationBuilder.DropTable(
                name: "AbpSettings");

            migrationBuilder.DropTable(
                name: "AbpUserClaims");

            migrationBuilder.DropTable(
                name: "AbpUserLogins");

            migrationBuilder.DropTable(
                name: "AbpUserRoles");

            migrationBuilder.DropTable(
                name: "AbpUserTokens");

            migrationBuilder.DropTable(
                name: "AbpRoles");

            migrationBuilder.DropTable(
                name: "AbpUsers");
        }
    }
}
