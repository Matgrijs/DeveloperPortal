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
                    { new Guid("053624f5-8e03-4058-825c-5f4b0472b18b"), "Hey developer!!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Developer", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("4acd9839-f289-47f4-8a3f-942edbb68d46"), "Hey developer, leuk dat je er bent!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Matthijs Meijboom", "google-oauth2|112201690251789498274" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("089cbe48-48b1-4a47-8f8d-54ad8af2d432"), "Applicatie afmaken!", "Developer", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("b2536b41-af56-47a0-a47a-143eb518f0b9"), "Demo voorbereiden!", "Matthijs Meijboom", "google-oauth2|112201690251789498274" }
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
