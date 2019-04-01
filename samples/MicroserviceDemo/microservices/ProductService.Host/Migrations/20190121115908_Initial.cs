using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductService.Host.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Price = table.Column<float>(nullable: false),
                    StockCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmProducts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PmProducts_Code",
                table: "PmProducts",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_PmProducts_Name",
                table: "PmProducts",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmProducts");
        }
    }
}
