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
                    { new Guid("2453fe8c-5de2-42cf-a393-e25ab7561832"), "Hey developer, leuk dat je er bent!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Matthijs Meijboom", "google-oauth2|112201690251789498274" },
                    { new Guid("6540898b-c70c-4625-9a01-92d437be9002"), "Hey developer!!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Developer", "auth0|661834a129d6402ebd9baa0c" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("727ceb19-d2ab-47ce-9a28-9acde3475c43"), "Demo voorbereiden!", "Matthijs Meijboom", "google-oauth2|112201690251789498274" },
                    { new Guid("aca74b08-d1c1-4c48-a17e-77b0e03b0ece"), "Applicatie afmaken!", "Developer", "auth0|661834a129d6402ebd9baa0c" }
                });

            migrationBuilder.InsertData(
                table: "PokerVotes",
                columns: new[] { "Id", "Username", "Vote", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("2d601ffb-69a4-4317-a1d7-33a0d2fd4908"), "Developer", "8", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("6c6d7539-632f-4f1f-ad2e-d7e145c84ec5"), "Matthijs Meijboom", "☕", "google-oauth2|112201690251789498274" }
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
