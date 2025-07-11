using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserBookandUserBookInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WantToRead",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBook",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBook", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_UserBook_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBook_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Title" },
                values: new object[] { new Guid("603e74b2-512a-432c-b9d5-1db379a0eb0e"), "Leo Tolstoy", "War and Peace" });

            migrationBuilder.InsertData(
                table: "UserBook",
                columns: new[] { "BookId", "UserId", "DateAdded", "Note" },
                values: new object[] { new Guid("603e74b2-512a-432c-b9d5-1db379a0eb0e"), new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"), new DateTime(2025, 6, 24, 17, 3, 28, 901, DateTimeKind.Local).AddTicks(7489), "Trying out adding a want to read value" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBook_BookId",
                table: "UserBook",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBook");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.AddColumn<string>(
                name: "WantToRead",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"),
                column: "WantToRead",
                value: null);
        }
    }
}
