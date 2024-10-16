using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class addProfilePic2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f890456-6ef5-4884-ba48-07eded1dbcf3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a899a49-786f-4d73-99e7-55610108f3e4");

            migrationBuilder.AddColumn<string>(
                name: "picture",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "10122251-794a-4e47-afd7-7645bf530a11", null, "admin", "ADMIN" },
                    { "da871ad6-1518-4527-b891-e1370183499d", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10122251-794a-4e47-afd7-7645bf530a11");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da871ad6-1518-4527-b891-e1370183499d");

            migrationBuilder.DropColumn(
                name: "picture",
                table: "Message");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f890456-6ef5-4884-ba48-07eded1dbcf3", null, "admin", "ADMIN" },
                    { "5a899a49-786f-4d73-99e7-55610108f3e4", null, "user", "USER" }
                });
        }
    }
}
