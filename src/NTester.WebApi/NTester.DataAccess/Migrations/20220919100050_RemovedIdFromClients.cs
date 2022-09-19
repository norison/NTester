using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTester.DataAccess.Migrations
{
    public partial class RemovedIdFromClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Clients_ClientId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ClientId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "RefreshTokens",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Name");

            migrationBuilder.InsertData(
                table: "Clients",
                column: "Name",
                value: "Swagger");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ClientName",
                table: "RefreshTokens",
                column: "ClientName");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_ClientName",
                table: "RefreshTokens",
                columns: new[] { "UserId", "ClientName" },
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Token", "ExpirationDateTime" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Clients_ClientName",
                table: "RefreshTokens",
                column: "ClientName",
                principalTable: "Clients",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Clients_ClientName",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_ClientName",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId_ClientName",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Name",
                keyValue: "NTester Web App");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Name",
                keyValue: "Postman");

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Name",
                keyValue: "Swagger");

            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "RefreshTokens");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("14147a39-737f-49af-b75d-44eba6b61885"), "NTester Web App" });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("5f730eaf-544e-423a-b24b-59a37a3155d6"), "Postman" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ClientId",
                table: "RefreshTokens",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens",
                columns: new[] { "UserId", "ClientId" },
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Token", "ExpirationDateTime" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Clients_ClientId",
                table: "RefreshTokens",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
