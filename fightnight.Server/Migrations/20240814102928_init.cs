using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84c2be24-ba86-4bd6-9396-8a3e7e5b8a6a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8538acd8-3c96-48a9-a220-e7f63adf7c1c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "23211203-0324-4763-a302-8b5855a0b7a5", null, "admin", "ADMIN" },
                    { "37f75041-4632-4676-b7de-8eb47cc99d33", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23211203-0324-4763-a302-8b5855a0b7a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "37f75041-4632-4676-b7de-8eb47cc99d33");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "84c2be24-ba86-4bd6-9396-8a3e7e5b8a6a", null, "user", "USER" },
                    { "8538acd8-3c96-48a9-a220-e7f63adf7c1c", null, "admin", "ADMIN" }
                });
        }
    }
}
