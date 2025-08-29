using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConversationMemberSchemaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ConversationMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ConversationMembers");
        }
    }
}
