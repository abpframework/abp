using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volo.CmsKit.Migrations
{
    public partial class IsActiveonmodules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CmsMenuItems_CmsMenus_MenuId",
                table: "CmsMenuItems");

            migrationBuilder.DropTable(
                name: "CmsMenus");

            migrationBuilder.DropIndex(
                name: "IX_CmsMenuItems_MenuId",
                table: "CmsMenuItems");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "CmsMenuItems");

            migrationBuilder.RenameColumn(
                name: "RequiredPermissionName",
                table: "CmsMenuItems",
                newName: "ExtraProperties");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CmsUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CmsMenuItems",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "CmsMenuItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "CmsComments",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "CmsComments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CmsUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CmsMenuItems");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CmsMenuItems");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "CmsComments");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "CmsComments");

            migrationBuilder.RenameColumn(
                name: "ExtraProperties",
                table: "CmsMenuItems",
                newName: "RequiredPermissionName");

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "CmsMenuItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CmsMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsMenus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CmsMenuItems_MenuId",
                table: "CmsMenuItems",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_CmsMenuItems_CmsMenus_MenuId",
                table: "CmsMenuItems",
                column: "MenuId",
                principalTable: "CmsMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
