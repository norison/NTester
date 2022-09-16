using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTester.DataAccess.Migrations
{
    public partial class AddedClientDefaultData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("14147a39-737f-49af-b75d-44eba6b61885"), "NTester Web App" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("5f730eaf-544e-423a-b24b-59a37a3155d6"), "Postman" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("14147a39-737f-49af-b75d-44eba6b61885"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("5f730eaf-544e-423a-b24b-59a37a3155d6"));
        }
    }
}
