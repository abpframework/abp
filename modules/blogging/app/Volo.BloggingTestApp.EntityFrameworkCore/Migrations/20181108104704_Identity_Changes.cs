using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.BloggingTestApp.EntityFrameworkCore.Migrations
{
    public partial class Identity_Changes : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AbpRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "AbpRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "AbpRoles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AbpClaimTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    Regex = table.Column<string>(maxLength: 512, nullable: true),
                    RegexDescription = table.Column<string>(maxLength: 128, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    ValueType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpClaimTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpClaimTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlgUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "BlgUsers");

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

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "AbpRoles");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "AbpRoles");
        }
    }
}
