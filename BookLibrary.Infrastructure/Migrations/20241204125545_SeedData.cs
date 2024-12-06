using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("a362ed9e-46fd-40e8-b6cf-8a945993e25e"), "George R.R. Martin" },
                    { new Guid("ad789cfb-4bfc-48a6-87ae-0dcc9037dcb9"), "J.K. Rowling" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Title", "Year" },
                values: new object[,]
                {
                    { new Guid("e36660d0-156a-4d5b-ac1c-f05941966d48"), new Guid("ad789cfb-4bfc-48a6-87ae-0dcc9037dcb9"), "Harry Potter", 1997 },
                    { new Guid("fe57c76f-a346-4556-aad9-635fd67081cc"), new Guid("a362ed9e-46fd-40e8-b6cf-8a945993e25e"), "Game of Thrones", 1996 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("e36660d0-156a-4d5b-ac1c-f05941966d48"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("fe57c76f-a346-4556-aad9-635fd67081cc"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("a362ed9e-46fd-40e8-b6cf-8a945993e25e"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("ad789cfb-4bfc-48a6-87ae-0dcc9037dcb9"));
        }
    }
}
