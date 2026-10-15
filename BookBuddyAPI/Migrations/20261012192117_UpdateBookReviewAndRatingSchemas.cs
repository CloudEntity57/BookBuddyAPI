using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookReviewAndRatingSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiId",
                table: "BookReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiId",
                table: "BookRatings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiId",
                table: "BookReviews");

            migrationBuilder.DropColumn(
                name: "ApiId",
                table: "BookRatings");
        }
    }
}
