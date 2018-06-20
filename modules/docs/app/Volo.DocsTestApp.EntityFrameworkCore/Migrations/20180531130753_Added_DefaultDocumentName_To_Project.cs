using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Volo.DocsTestApp.EntityFrameworkCore.Migrations
{
    public partial class Added_DefaultDocumentName_To_Project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultDocumentName",
                table: "DocsProjects",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultDocumentName",
                table: "DocsProjects");
        }
    }
}
