using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeveloperPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    auth0Id = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    MessageTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    auth0Id = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PokerVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    auth0Id = table.Column<string>(type: "text", nullable: false),
                    Vote = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokerVotes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Message", "MessageTime", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("09806674-62db-428a-8aa2-669c18268b82"), "Hey developer!!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Developer", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("e599efb5-b943-4f8f-893f-2d8bc6f82d61"), "Hey developer, leuk dat je er bent!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Matthijs Meijboom", "google-oauth2|112201690251789498274" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("6bc63911-27d2-4bc4-94f2-22b4d45ffd8a"), "Applicatie afmaken!", "Developer", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("fc7b557f-1728-48c1-8da0-816d514d53df"), "Demo voorbereiden!", "Matthijs Meijboom", "google-oauth2|112201690251789498274" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PokerVotes");
        }
    }
}
