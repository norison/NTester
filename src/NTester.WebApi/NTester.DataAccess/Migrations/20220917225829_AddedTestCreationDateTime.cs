using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTester.DataAccess.Migrations
{
    public partial class AddedTestCreationDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "Tests");
        }
    }
}
