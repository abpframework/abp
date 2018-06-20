using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.DocsTestApp.EntityFrameworkCore.Migrations
{
    public partial class Added_ShortName_And_DocumentStoreType_To_Project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentStoreType",
                table: "DocsProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "DocsProjects",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentStoreType",
                table: "DocsProjects");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "DocsProjects");
        }
    }
}
