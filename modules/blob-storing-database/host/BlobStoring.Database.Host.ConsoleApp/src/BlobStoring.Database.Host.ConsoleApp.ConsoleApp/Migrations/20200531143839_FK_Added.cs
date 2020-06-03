using Microsoft.EntityFrameworkCore.Migrations;

namespace BlobStoring.Database.Host.ConsoleApp.ConsoleApp.Migrations
{
    public partial class FK_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AbpBlobs_ContainerId",
                table: "AbpBlobs",
                column: "ContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpBlobs_AbpBlobContainers_ContainerId",
                table: "AbpBlobs",
                column: "ContainerId",
                principalTable: "AbpBlobContainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpBlobs_AbpBlobContainers_ContainerId",
                table: "AbpBlobs");

            migrationBuilder.DropIndex(
                name: "IX_AbpBlobs_ContainerId",
                table: "AbpBlobs");
        }
    }
}
