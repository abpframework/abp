using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.AbpWebSite.EntityFrameworkCore.Migrations
{
    public partial class Module_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlgUsers",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "BlgUsers",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "BlgTags",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AbpUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AbpUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AbpUsers",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AbpUsers",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlgUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "BlgUsers");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AbpUsers");
        }
    }
}
