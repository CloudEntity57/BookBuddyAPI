using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class SchemaChanges0718 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BookDTOUserDTO");

            //migrationBuilder.DropTable(
            //    name: "BookDTO");

            //migrationBuilder.DropTable(
            //    name: "UserDTO");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("603e74b2-512a-432c-b9d5-1db379a0eb0e"));

            migrationBuilder.DeleteData(
                table: "BuddyRequest",
                keyColumns: new[] { "ActiveUserId", "PassiveUserId" },
                keyValues: new object[] { new Guid("b8e244db-c0a5-440b-990f-50d65ac34add"), new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e") });

            migrationBuilder.AddColumn<string>(
                name: "BookOfInterest",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserMessage",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateAdded",
                table: "BuddyRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookTitle",
                table: "BuddyRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookOfInterest",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserMessage",
                table: "Users");



            migrationBuilder.AlterColumn<DateTime>(
                name: "DateAdded",
                table: "BuddyRequest",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");





            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Title" },
                values: new object[] { new Guid("603e74b2-512a-432c-b9d5-1db379a0eb0e"), "Leo Tolstoy", "War and Peace" });

            migrationBuilder.InsertData(
                table: "BuddyRequest",
                columns: new[] { "ActiveUserId", "PassiveUserId", "DateAdded", "Note" },
                values: new object[] { new Guid("b8e244db-c0a5-440b-990f-50d65ac34add"), new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"), new DateTime(2025, 6, 24, 17, 3, 28, 901, DateTimeKind.Local).AddTicks(7489), "Trying out adding Ogre as a new buddy request value" });


        }
    }
}
