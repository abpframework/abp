using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class RenameIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnitRoles_AbpOrganizationUnits_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnitRoles_AbpRoles_RoleId",
                table: "AbpOrganizationUnitRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                table: "AbpRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserClaims_AbpUsers_UserId",
                table: "AbpUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserLogins_AbpUsers_UserId",
                table: "AbpUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserOrganizationUnits_AbpOrganizationUnits_OrganizationUnitId",
                table: "AbpUserOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserOrganizationUnits_AbpUsers_UserId",
                table: "AbpUserOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserRoles_AbpRoles_RoleId",
                table: "AbpUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserRoles_AbpUsers_UserId",
                table: "AbpUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AbpUserTokens_AbpUsers_UserId",
                table: "AbpUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpUserTokens",
                table: "AbpUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpUsers",
                table: "AbpUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpUserRoles",
                table: "AbpUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpUserOrganizationUnits",
                table: "AbpUserOrganizationUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpUserLogins",
                table: "AbpUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpUserClaims",
                table: "AbpUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpSecurityLogs",
                table: "AbpSecurityLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpRoles",
                table: "AbpRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpRoleClaims",
                table: "AbpRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpOrganizationUnits",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpOrganizationUnitRoles",
                table: "AbpOrganizationUnitRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpLinkUsers",
                table: "AbpLinkUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbpClaimTypes",
                table: "AbpClaimTypes");

            migrationBuilder.RenameTable(
                name: "AbpUserTokens",
                newName: "MyIdentityUserTokens");

            migrationBuilder.RenameTable(
                name: "AbpUsers",
                newName: "MyIdentityUsers");

            migrationBuilder.RenameTable(
                name: "AbpUserRoles",
                newName: "MyIdentityUserRoles");

            migrationBuilder.RenameTable(
                name: "AbpUserOrganizationUnits",
                newName: "MyIdentityUserOrganizationUnits");

            migrationBuilder.RenameTable(
                name: "AbpUserLogins",
                newName: "MyIdentityUserLogins");

            migrationBuilder.RenameTable(
                name: "AbpUserClaims",
                newName: "MyIdentityUserClaims");

            migrationBuilder.RenameTable(
                name: "AbpSecurityLogs",
                newName: "MyIdentitySecurityLogs");

            migrationBuilder.RenameTable(
                name: "AbpRoles",
                newName: "MyIdentityRoles");

            migrationBuilder.RenameTable(
                name: "AbpRoleClaims",
                newName: "MyIdentityRoleClaims");

            migrationBuilder.RenameTable(
                name: "AbpOrganizationUnits",
                newName: "MyIdentityOrganizationUnits");

            migrationBuilder.RenameTable(
                name: "AbpOrganizationUnitRoles",
                newName: "MyIdentityOrganizationUnitRoles");

            migrationBuilder.RenameTable(
                name: "AbpLinkUsers",
                newName: "MyIdentityLinkUsers");

            migrationBuilder.RenameTable(
                name: "AbpClaimTypes",
                newName: "MyIdentityClaimTypes");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUsers_UserName",
                table: "MyIdentityUsers",
                newName: "IX_MyIdentityUsers_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUsers_NormalizedUserName",
                table: "MyIdentityUsers",
                newName: "IX_MyIdentityUsers_NormalizedUserName");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUsers_NormalizedEmail",
                table: "MyIdentityUsers",
                newName: "IX_MyIdentityUsers_NormalizedEmail");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUsers_Email",
                table: "MyIdentityUsers",
                newName: "IX_MyIdentityUsers_Email");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUserRoles_RoleId_UserId",
                table: "MyIdentityUserRoles",
                newName: "IX_MyIdentityUserRoles_RoleId_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUserOrganizationUnits_UserId_OrganizationUnitId",
                table: "MyIdentityUserOrganizationUnits",
                newName: "IX_MyIdentityUserOrganizationUnits_UserId_OrganizationUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUserLogins_LoginProvider_ProviderKey",
                table: "MyIdentityUserLogins",
                newName: "IX_MyIdentityUserLogins_LoginProvider_ProviderKey");

            migrationBuilder.RenameIndex(
                name: "IX_AbpUserClaims_UserId",
                table: "MyIdentityUserClaims",
                newName: "IX_MyIdentityUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpSecurityLogs_TenantId_UserId",
                table: "MyIdentitySecurityLogs",
                newName: "IX_MyIdentitySecurityLogs_TenantId_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpSecurityLogs_TenantId_Identity",
                table: "MyIdentitySecurityLogs",
                newName: "IX_MyIdentitySecurityLogs_TenantId_Identity");

            migrationBuilder.RenameIndex(
                name: "IX_AbpSecurityLogs_TenantId_ApplicationName",
                table: "MyIdentitySecurityLogs",
                newName: "IX_MyIdentitySecurityLogs_TenantId_ApplicationName");

            migrationBuilder.RenameIndex(
                name: "IX_AbpSecurityLogs_TenantId_Action",
                table: "MyIdentitySecurityLogs",
                newName: "IX_MyIdentitySecurityLogs_TenantId_Action");

            migrationBuilder.RenameIndex(
                name: "IX_AbpRoles_NormalizedName",
                table: "MyIdentityRoles",
                newName: "IX_MyIdentityRoles_NormalizedName");

            migrationBuilder.RenameIndex(
                name: "IX_AbpRoleClaims_RoleId",
                table: "MyIdentityRoleClaims",
                newName: "IX_MyIdentityRoleClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpOrganizationUnits_ParentId",
                table: "MyIdentityOrganizationUnits",
                newName: "IX_MyIdentityOrganizationUnits_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpOrganizationUnits_Code",
                table: "MyIdentityOrganizationUnits",
                newName: "IX_MyIdentityOrganizationUnits_Code");

            migrationBuilder.RenameIndex(
                name: "IX_AbpOrganizationUnitRoles_RoleId_OrganizationUnitId",
                table: "MyIdentityOrganizationUnitRoles",
                newName: "IX_MyIdentityOrganizationUnitRoles_RoleId_OrganizationUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId",
                table: "MyIdentityLinkUsers",
                newName: "IX_MyIdentityLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityUserTokens",
                table: "MyIdentityUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityUsers",
                table: "MyIdentityUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityUserRoles",
                table: "MyIdentityUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityUserOrganizationUnits",
                table: "MyIdentityUserOrganizationUnits",
                columns: new[] { "OrganizationUnitId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityUserLogins",
                table: "MyIdentityUserLogins",
                columns: new[] { "UserId", "LoginProvider" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityUserClaims",
                table: "MyIdentityUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentitySecurityLogs",
                table: "MyIdentitySecurityLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityRoles",
                table: "MyIdentityRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityRoleClaims",
                table: "MyIdentityRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityOrganizationUnits",
                table: "MyIdentityOrganizationUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityOrganizationUnitRoles",
                table: "MyIdentityOrganizationUnitRoles",
                columns: new[] { "OrganizationUnitId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityLinkUsers",
                table: "MyIdentityLinkUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyIdentityClaimTypes",
                table: "MyIdentityClaimTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityOrganizationUnitRoles_MyIdentityOrganizationUnits_OrganizationUnitId",
                table: "MyIdentityOrganizationUnitRoles",
                column: "OrganizationUnitId",
                principalTable: "MyIdentityOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityOrganizationUnitRoles_MyIdentityRoles_RoleId",
                table: "MyIdentityOrganizationUnitRoles",
                column: "RoleId",
                principalTable: "MyIdentityRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityOrganizationUnits_MyIdentityOrganizationUnits_ParentId",
                table: "MyIdentityOrganizationUnits",
                column: "ParentId",
                principalTable: "MyIdentityOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityRoleClaims_MyIdentityRoles_RoleId",
                table: "MyIdentityRoleClaims",
                column: "RoleId",
                principalTable: "MyIdentityRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityUserClaims_MyIdentityUsers_UserId",
                table: "MyIdentityUserClaims",
                column: "UserId",
                principalTable: "MyIdentityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityUserLogins_MyIdentityUsers_UserId",
                table: "MyIdentityUserLogins",
                column: "UserId",
                principalTable: "MyIdentityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityUserOrganizationUnits_MyIdentityOrganizationUnits_OrganizationUnitId",
                table: "MyIdentityUserOrganizationUnits",
                column: "OrganizationUnitId",
                principalTable: "MyIdentityOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityUserOrganizationUnits_MyIdentityUsers_UserId",
                table: "MyIdentityUserOrganizationUnits",
                column: "UserId",
                principalTable: "MyIdentityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityUserRoles_MyIdentityRoles_RoleId",
                table: "MyIdentityUserRoles",
                column: "RoleId",
                principalTable: "MyIdentityRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityUserRoles_MyIdentityUsers_UserId",
                table: "MyIdentityUserRoles",
                column: "UserId",
                principalTable: "MyIdentityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MyIdentityUserTokens_MyIdentityUsers_UserId",
                table: "MyIdentityUserTokens",
                column: "UserId",
                principalTable: "MyIdentityUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityOrganizationUnitRoles_MyIdentityOrganizationUnits_OrganizationUnitId",
                table: "MyIdentityOrganizationUnitRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityOrganizationUnitRoles_MyIdentityRoles_RoleId",
                table: "MyIdentityOrganizationUnitRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityOrganizationUnits_MyIdentityOrganizationUnits_ParentId",
                table: "MyIdentityOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityRoleClaims_MyIdentityRoles_RoleId",
                table: "MyIdentityRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityUserClaims_MyIdentityUsers_UserId",
                table: "MyIdentityUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityUserLogins_MyIdentityUsers_UserId",
                table: "MyIdentityUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityUserOrganizationUnits_MyIdentityOrganizationUnits_OrganizationUnitId",
                table: "MyIdentityUserOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityUserOrganizationUnits_MyIdentityUsers_UserId",
                table: "MyIdentityUserOrganizationUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityUserRoles_MyIdentityRoles_RoleId",
                table: "MyIdentityUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityUserRoles_MyIdentityUsers_UserId",
                table: "MyIdentityUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_MyIdentityUserTokens_MyIdentityUsers_UserId",
                table: "MyIdentityUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityUserTokens",
                table: "MyIdentityUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityUsers",
                table: "MyIdentityUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityUserRoles",
                table: "MyIdentityUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityUserOrganizationUnits",
                table: "MyIdentityUserOrganizationUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityUserLogins",
                table: "MyIdentityUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityUserClaims",
                table: "MyIdentityUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentitySecurityLogs",
                table: "MyIdentitySecurityLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityRoles",
                table: "MyIdentityRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityRoleClaims",
                table: "MyIdentityRoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityOrganizationUnits",
                table: "MyIdentityOrganizationUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityOrganizationUnitRoles",
                table: "MyIdentityOrganizationUnitRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityLinkUsers",
                table: "MyIdentityLinkUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyIdentityClaimTypes",
                table: "MyIdentityClaimTypes");

            migrationBuilder.RenameTable(
                name: "MyIdentityUserTokens",
                newName: "AbpUserTokens");

            migrationBuilder.RenameTable(
                name: "MyIdentityUsers",
                newName: "AbpUsers");

            migrationBuilder.RenameTable(
                name: "MyIdentityUserRoles",
                newName: "AbpUserRoles");

            migrationBuilder.RenameTable(
                name: "MyIdentityUserOrganizationUnits",
                newName: "AbpUserOrganizationUnits");

            migrationBuilder.RenameTable(
                name: "MyIdentityUserLogins",
                newName: "AbpUserLogins");

            migrationBuilder.RenameTable(
                name: "MyIdentityUserClaims",
                newName: "AbpUserClaims");

            migrationBuilder.RenameTable(
                name: "MyIdentitySecurityLogs",
                newName: "AbpSecurityLogs");

            migrationBuilder.RenameTable(
                name: "MyIdentityRoles",
                newName: "AbpRoles");

            migrationBuilder.RenameTable(
                name: "MyIdentityRoleClaims",
                newName: "AbpRoleClaims");

            migrationBuilder.RenameTable(
                name: "MyIdentityOrganizationUnits",
                newName: "AbpOrganizationUnits");

            migrationBuilder.RenameTable(
                name: "MyIdentityOrganizationUnitRoles",
                newName: "AbpOrganizationUnitRoles");

            migrationBuilder.RenameTable(
                name: "MyIdentityLinkUsers",
                newName: "AbpLinkUsers");

            migrationBuilder.RenameTable(
                name: "MyIdentityClaimTypes",
                newName: "AbpClaimTypes");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUsers_UserName",
                table: "AbpUsers",
                newName: "IX_AbpUsers_UserName");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUsers_NormalizedUserName",
                table: "AbpUsers",
                newName: "IX_AbpUsers_NormalizedUserName");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUsers_NormalizedEmail",
                table: "AbpUsers",
                newName: "IX_AbpUsers_NormalizedEmail");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUsers_Email",
                table: "AbpUsers",
                newName: "IX_AbpUsers_Email");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUserRoles_RoleId_UserId",
                table: "AbpUserRoles",
                newName: "IX_AbpUserRoles_RoleId_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUserOrganizationUnits_UserId_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                newName: "IX_AbpUserOrganizationUnits_UserId_OrganizationUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUserLogins_LoginProvider_ProviderKey",
                table: "AbpUserLogins",
                newName: "IX_AbpUserLogins_LoginProvider_ProviderKey");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityUserClaims_UserId",
                table: "AbpUserClaims",
                newName: "IX_AbpUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentitySecurityLogs_TenantId_UserId",
                table: "AbpSecurityLogs",
                newName: "IX_AbpSecurityLogs_TenantId_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentitySecurityLogs_TenantId_Identity",
                table: "AbpSecurityLogs",
                newName: "IX_AbpSecurityLogs_TenantId_Identity");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentitySecurityLogs_TenantId_ApplicationName",
                table: "AbpSecurityLogs",
                newName: "IX_AbpSecurityLogs_TenantId_ApplicationName");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentitySecurityLogs_TenantId_Action",
                table: "AbpSecurityLogs",
                newName: "IX_AbpSecurityLogs_TenantId_Action");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityRoles_NormalizedName",
                table: "AbpRoles",
                newName: "IX_AbpRoles_NormalizedName");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityRoleClaims_RoleId",
                table: "AbpRoleClaims",
                newName: "IX_AbpRoleClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                newName: "IX_AbpOrganizationUnits_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityOrganizationUnits_Code",
                table: "AbpOrganizationUnits",
                newName: "IX_AbpOrganizationUnits_Code");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityOrganizationUnitRoles_RoleId_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                newName: "IX_AbpOrganizationUnitRoles_RoleId_OrganizationUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_MyIdentityLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId",
                table: "AbpLinkUsers",
                newName: "IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_TargetTenantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpUserTokens",
                table: "AbpUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpUsers",
                table: "AbpUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpUserRoles",
                table: "AbpUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpUserOrganizationUnits",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "OrganizationUnitId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpUserLogins",
                table: "AbpUserLogins",
                columns: new[] { "UserId", "LoginProvider" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpUserClaims",
                table: "AbpUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpSecurityLogs",
                table: "AbpSecurityLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpRoles",
                table: "AbpRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpRoleClaims",
                table: "AbpRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpOrganizationUnits",
                table: "AbpOrganizationUnits",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpOrganizationUnitRoles",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "OrganizationUnitId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpLinkUsers",
                table: "AbpLinkUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbpClaimTypes",
                table: "AbpClaimTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnitRoles_AbpOrganizationUnits_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                column: "OrganizationUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnitRoles_AbpRoles_RoleId",
                table: "AbpOrganizationUnitRoles",
                column: "RoleId",
                principalTable: "AbpRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                column: "ParentId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpRoleClaims_AbpRoles_RoleId",
                table: "AbpRoleClaims",
                column: "RoleId",
                principalTable: "AbpRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserClaims_AbpUsers_UserId",
                table: "AbpUserClaims",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserLogins_AbpUsers_UserId",
                table: "AbpUserLogins",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserOrganizationUnits_AbpOrganizationUnits_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                column: "OrganizationUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserOrganizationUnits_AbpUsers_UserId",
                table: "AbpUserOrganizationUnits",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserRoles_AbpRoles_RoleId",
                table: "AbpUserRoles",
                column: "RoleId",
                principalTable: "AbpRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserRoles_AbpUsers_UserId",
                table: "AbpUserRoles",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUserTokens_AbpUsers_UserId",
                table: "AbpUserTokens",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
