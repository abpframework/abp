using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrganizationService.Host.Migrations
{
    public partial class INIT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpOrganizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserOrganizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserOrganizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpUserOrganizations_AbpOrganizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "AbpOrganizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizations_OrganizationId",
                table: "AbpUserOrganizations",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpUserOrganizations");

            migrationBuilder.DropTable(
                name: "AbpOrganizations");
        }
    }
}
