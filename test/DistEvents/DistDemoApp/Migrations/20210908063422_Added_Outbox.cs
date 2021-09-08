using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DistDemoApp.Migrations
{
    public partial class Added_Outbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpEventOutbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EventData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpEventOutbox", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpEventOutbox");
        }
    }
}
