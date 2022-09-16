using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTester.DataAccess.Migrations
{
    public partial class AddedAlternateKeysForRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RefreshTokens_AccessToken",
                table: "RefreshTokens",
                column: "AccessToken");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens",
                columns: new[] { "UserId", "ClientId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RefreshTokens_AccessToken",
                table: "RefreshTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RefreshTokens_UserId_ClientId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                columns: new[] { "Token", "AccessToken" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }
    }
}
