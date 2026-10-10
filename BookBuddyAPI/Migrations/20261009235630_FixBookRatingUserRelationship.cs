using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixBookRatingUserRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRatings_Users_UserId1",
                table: "BookRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_BookReviews_Users_UserId1",
                table: "BookReviews");

            migrationBuilder.DropIndex(
                name: "IX_BookReviews_UserId1",
                table: "BookReviews");

            migrationBuilder.DropIndex(
                name: "IX_BookRatings_UserId1",
                table: "BookRatings");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BookReviews");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BookRatings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "BookReviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "BookRatings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookReviews_UserId1",
                table: "BookReviews",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_BookRatings_UserId1",
                table: "BookRatings",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRatings_Users_UserId1",
                table: "BookRatings",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReviews_Users_UserId1",
                table: "BookReviews",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
