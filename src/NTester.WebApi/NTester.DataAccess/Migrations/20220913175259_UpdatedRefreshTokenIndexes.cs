using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTester.DataAccess.Migrations
{
    public partial class UpdatedRefreshTokenIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RefreshTokens_AccessToken",
                table: "RefreshTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AccessToken",
                table: "RefreshTokens",
                column: "AccessToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens",
                columns: new[] { "UserId", "ClientId" },
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Token", "AccessToken", "ExpirationDateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_AccessToken",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RefreshTokens_AccessToken",
                table: "RefreshTokens",
                column: "AccessToken");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens",
                columns: new[] { "UserId", "ClientId" });
        }
    }
}
