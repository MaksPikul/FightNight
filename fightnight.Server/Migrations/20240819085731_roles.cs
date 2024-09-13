using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "099f8824-0d91-4e66-8287-b46f343aba29");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ba50e8a-50a2-4a64-a44e-72c11619a3ed");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "AppUserEvent",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "139e1020-2e1d-4d6c-b6f7-6bb15fde2b25", null, "user", "USER" },
                    { "8819b246-a25c-4fd5-b697-de5dac0f6a24", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "139e1020-2e1d-4d6c-b6f7-6bb15fde2b25");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8819b246-a25c-4fd5-b697-de5dac0f6a24");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AppUserEvent");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "099f8824-0d91-4e66-8287-b46f343aba29", null, "user", "USER" },
                    { "1ba50e8a-50a2-4a64-a44e-72c11619a3ed", null, "admin", "ADMIN" }
                });
        }
    }
}
