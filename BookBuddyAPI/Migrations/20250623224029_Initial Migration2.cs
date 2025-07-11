using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0a61fd82-0dd4-4544-bc16-63d192e2590f"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "CreatedAt", "Email", "FirstName", "LastLoginAt", "LastName", "UserName", "WantToRead" },
                values: new object[] { new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"), null, new DateTime(2025, 6, 23, 17, 40, 28, 901, DateTimeKind.Local).AddTicks(7489), "joshthewise57@gmail.com", null, null, null, "HelioMoonWave Literati", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "CreatedAt", "Email", "FirstName", "LastLoginAt", "LastName", "UserName", "WantToRead" },
                values: new object[] { new Guid("0a61fd82-0dd4-4544-bc16-63d192e2590f"), null, new DateTime(2025, 6, 23, 17, 34, 34, 16, DateTimeKind.Local).AddTicks(944), "joshthewise57@gmail.com", null, null, null, "HelioMoonWave Literati", null });
        }
    }
}
