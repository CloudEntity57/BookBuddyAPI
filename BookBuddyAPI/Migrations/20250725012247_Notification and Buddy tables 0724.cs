using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class NotificationandBuddytables0724 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Buddies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buddies_UserId",
                table: "Buddies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buddies_Users_UserId",
                table: "Buddies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buddies_Users_UserId",
                table: "Buddies");

            migrationBuilder.DropIndex(
                name: "IX_Buddies_UserId",
                table: "Buddies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Buddies");
        }
    }
}
