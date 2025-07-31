using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class BuddyRequestJoinMigration11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "BuddyRequestJoin");

            migrationBuilder.CreateTable(
                name: "BuddyRequest",
                columns: table => new
                {
                    ActiveUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PassiveUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuddyRequest", x => new { x.ActiveUserId, x.PassiveUserId });
                    table.ForeignKey(
                        name: "FK_BuddyRequest_Users_ActiveUserId",
                        column: x => x.ActiveUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BuddyRequest_Users_PassiveUserId",
                        column: x => x.PassiveUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "BuddyRequest",
                columns: new[] { "ActiveUserId", "PassiveUserId", "DateAdded", "Note" },
                values: new object[] { new Guid("b8e244db-c0a5-440b-990f-50d65ac34add"), new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"), new DateTime(2025, 6, 24, 17, 3, 28, 901, DateTimeKind.Local).AddTicks(7489), "Trying out adding Ogre as a new buddy request value" });

            migrationBuilder.CreateIndex(
                name: "IX_BuddyRequest_PassiveUserId",
                table: "BuddyRequest",
                column: "PassiveUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuddyRequest");

            //migrationBuilder.CreateTable(
            //    name: "BuddyRequestJoin",
            //    columns: table => new
            //    {
            //        ActiveUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        PassiveUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BuddyRequestJoin", x => new { x.ActiveUserId, x.PassiveUserId });
            //        table.ForeignKey(
            //            name: "FK_BuddyRequestJoin_Users_ActiveUserId",
            //            column: x => x.ActiveUserId,
            //            principalTable: "Users",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_BuddyRequestJoin_Users_PassiveUserId",
            //            column: x => x.PassiveUserId,
            //            principalTable: "Users",
            //            principalColumn: "Id");
            //    });

            migrationBuilder.InsertData(
                table: "BuddyRequestJoin",
                columns: new[] { "ActiveUserId", "PassiveUserId", "DateAdded", "Note" },
                values: new object[] { new Guid("b8e244db-c0a5-440b-990f-50d65ac34add"), new Guid("603e74b2-511a-431c-b9d5-1db379a0eb0e"), new DateTime(2025, 6, 24, 17, 3, 28, 901, DateTimeKind.Local).AddTicks(7489), "Trying out adding Ogre as a new buddy request value" });

            migrationBuilder.CreateIndex(
                name: "IX_BuddyRequestJoin_PassiveUserId",
                table: "BuddyRequestJoin",
                column: "PassiveUserId");
        }
    }
}
