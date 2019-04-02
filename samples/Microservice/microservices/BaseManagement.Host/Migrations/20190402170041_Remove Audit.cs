using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseManagement.Host.Migrations
{
    public partial class RemoveAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "BmBaseTypes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "BmBaseTypes");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "BmBaseTypes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "BmBaseTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BmBaseTypes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "BmBaseTypes");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "BmBaseTypes");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "BmBaseItems");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "BmBaseItems");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "BmBaseItems");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "BmBaseItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BmBaseItems");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "BmBaseItems");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "BmBaseItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "BmBaseTypes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "BmBaseTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "BmBaseTypes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "BmBaseTypes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BmBaseTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "BmBaseTypes",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "BmBaseTypes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "BmBaseItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "BmBaseItems",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "BmBaseItems",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "BmBaseItems",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BmBaseItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "BmBaseItems",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "BmBaseItems",
                nullable: true);
        }
    }
}
