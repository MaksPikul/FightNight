using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class seperateDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "139e1020-2e1d-4d6c-b6f7-6bb15fde2b25");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8819b246-a25c-4fd5-b697-de5dac0f6a24");

            migrationBuilder.RenameColumn(
                name: "dateTime",
                table: "Event",
                newName: "date");

            migrationBuilder.AddColumn<int>(
                name: "numMatches",
                table: "Event",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "time",
                table: "Event",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f4d3c29-55ce-4b47-9635-c70739b89403", null, "user", "USER" },
                    { "186aebd7-c7a3-49b2-b825-5f79e35c498d", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f4d3c29-55ce-4b47-9635-c70739b89403");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "186aebd7-c7a3-49b2-b825-5f79e35c498d");

            migrationBuilder.DropColumn(
                name: "numMatches",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "time",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Event",
                newName: "dateTime");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "139e1020-2e1d-4d6c-b6f7-6bb15fde2b25", null, "user", "USER" },
                    { "8819b246-a25c-4fd5-b697-de5dac0f6a24", null, "admin", "ADMIN" }
                });
        }
    }
}
