using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Host.Migrations
{
    public partial class Audit_Log_Module_Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges");

            migrationBuilder.DropIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges");

            migrationBuilder.DropColumn(
                name: "EntityChangeId1",
                table: "AbpEntityPropertyChanges");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "AbpAuditLogs",
                maxLength: 96,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "AbpAuditLogs",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "AbpAuditLogs",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "AbpAuditLogs");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AbpAuditLogs");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "AbpAuditLogs");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityChangeId1",
                table: "AbpEntityPropertyChanges",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpEntityPropertyChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId1",
                table: "AbpEntityPropertyChanges",
                column: "EntityChangeId1",
                principalTable: "AbpEntityChanges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
