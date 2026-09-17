using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volo.Abp.BackgroundJobs.DemoApp.Migrations
{
    /// <inheritdoc />
    public partial class Added_CompletionTime_To_BackgroundJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionTime",
                table: "AbpBackgroundJobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_ApplicationName_CompletionTime_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "ApplicationName", "CompletionTime", "IsAbandoned", "NextTryTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpBackgroundJobs_ApplicationName_CompletionTime_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs");

            migrationBuilder.DropColumn(
                name: "CompletionTime",
                table: "AbpBackgroundJobs");

            migrationBuilder.CreateIndex(
                name: "IX_AbpBackgroundJobs_IsAbandoned_NextTryTime",
                table: "AbpBackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });
        }
    }
}
