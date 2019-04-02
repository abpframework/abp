using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseManagement.Host.Migrations
{
    public partial class Added_BaseType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmBaseTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmBaseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PmBaseItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    BaseTypeGuid = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 32, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmBaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmBaseItems_PmBaseTypes_BaseTypeGuid",
                        column: x => x.BaseTypeGuid,
                        principalTable: "PmBaseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PmBaseItems_BaseTypeGuid",
                table: "PmBaseItems",
                column: "BaseTypeGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmBaseItems");

            migrationBuilder.DropTable(
                name: "PmBaseTypes");
        }
    }
}
