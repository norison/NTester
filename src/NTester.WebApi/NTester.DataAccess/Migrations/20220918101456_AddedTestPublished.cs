using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTester.DataAccess.Migrations
{
    public partial class AddedTestPublished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Tests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "Tests");
        }
    }
}
