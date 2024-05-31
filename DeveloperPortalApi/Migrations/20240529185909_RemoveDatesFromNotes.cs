using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeveloperPortalApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDatesFromNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: new Guid("a344df65-d247-430a-8f1e-d05d6c79cc6f"));

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: new Guid("b7f59c91-48e0-4540-bd91-eff879ba223a"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("3bf60259-cab1-4758-97a8-e71f2913ef80"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("5f53f690-fd98-4a9c-98a3-6e5a5bec8b37"));

            migrationBuilder.DeleteData(
                table: "PokerVotes",
                keyColumn: "Id",
                keyValue: new Guid("44810ea7-46a7-4f52-8560-0bc6c774cee3"));

            migrationBuilder.DeleteData(
                table: "PokerVotes",
                keyColumn: "Id",
                keyValue: new Guid("e5519e8c-25e6-495a-a79a-7dc4fa4e6ae4"));

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Notes");

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Message", "MessageTime", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("b9250775-52ee-445a-b75b-e3e825c3decc"), "Hey Matthijs, fijn dat ik er ben!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Developer", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("bdeac4ba-9960-4c30-bbaa-791c4252bec8"), "Hey developer, leuk dat je er bent!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Matthijs", "auth0|661834a129d6402ebd9baa0c" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("1a38a3c0-d856-49b3-be01-99e9cbf24478"), "Sell the application!", "Developer", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("790a4aa5-ed1f-4f42-9d2d-3a6dd9bc1014"), "Applicatie afmaken!", "Matthijs", "auth0|661834a129d6402ebd9baa0c" }
                });

            migrationBuilder.InsertData(
                table: "PokerVotes",
                columns: new[] { "Id", "Username", "Vote", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("0997ac68-85c3-44bf-b78b-1cde8b46c5c4"), "Developer", "8", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("b95b7e26-e4fb-4b0e-8859-6d77f174aae5"), "Matthijs Meijboom", "☕", "auth0|661834a129d6402ebd9baa0c" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: new Guid("b9250775-52ee-445a-b75b-e3e825c3decc"));

            migrationBuilder.DeleteData(
                table: "ChatMessages",
                keyColumn: "Id",
                keyValue: new Guid("bdeac4ba-9960-4c30-bbaa-791c4252bec8"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("1a38a3c0-d856-49b3-be01-99e9cbf24478"));

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: new Guid("790a4aa5-ed1f-4f42-9d2d-3a6dd9bc1014"));

            migrationBuilder.DeleteData(
                table: "PokerVotes",
                keyColumn: "Id",
                keyValue: new Guid("0997ac68-85c3-44bf-b78b-1cde8b46c5c4"));

            migrationBuilder.DeleteData(
                table: "PokerVotes",
                keyColumn: "Id",
                keyValue: new Guid("b95b7e26-e4fb-4b0e-8859-6d77f174aae5"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Notes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DueDate",
                table: "Notes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "Id", "Message", "MessageTime", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("a344df65-d247-430a-8f1e-d05d6c79cc6f"), "Hey developer, leuk dat je er bent!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Matthijs", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("b7f59c91-48e0-4540-bd91-eff879ba223a"), "Hey Matthijs, fijn dat ik er ben!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Developer", "auth0|661834a129d6402ebd9baa0c" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "CreatedOn", "DueDate", "Username", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("3bf60259-cab1-4758-97a8-e71f2913ef80"), "Applicatie afmaken!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), null, "Matthijs", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("5f53f690-fd98-4a9c-98a3-6e5a5bec8b37"), "Sell the application!", new DateTimeOffset(new DateTime(2024, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), new DateTimeOffset(new DateTime(2025, 4, 7, 16, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Developer", "auth0|661834a129d6402ebd9baa0c" }
                });

            migrationBuilder.InsertData(
                table: "PokerVotes",
                columns: new[] { "Id", "Username", "Vote", "auth0Id" },
                values: new object[,]
                {
                    { new Guid("44810ea7-46a7-4f52-8560-0bc6c774cee3"), "Developer", "8", "auth0|661834a129d6402ebd9baa0c" },
                    { new Guid("e5519e8c-25e6-495a-a79a-7dc4fa4e6ae4"), "Matthijs Meijboom", "☕", "auth0|661834a129d6402ebd9baa0c" }
                });
        }
    }
}
