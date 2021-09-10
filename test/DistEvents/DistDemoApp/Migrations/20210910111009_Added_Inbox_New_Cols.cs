using Microsoft.EntityFrameworkCore.Migrations;

namespace DistDemoApp.Migrations
{
    public partial class Added_Inbox_New_Cols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "AbpEventInbox",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpEventInbox_MessageId",
                table: "AbpEventInbox",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpEventInbox_Processed_CreationTime",
                table: "AbpEventInbox",
                columns: new[] { "Processed", "CreationTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpEventInbox_MessageId",
                table: "AbpEventInbox");

            migrationBuilder.DropIndex(
                name: "IX_AbpEventInbox_Processed_CreationTime",
                table: "AbpEventInbox");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "AbpEventInbox");
        }
    }
}
