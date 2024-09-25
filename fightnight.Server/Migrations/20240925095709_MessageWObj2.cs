using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class MessageWObj2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2f34a52b-62fa-468d-bbc3-7fd3e23d4e9a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b170f49-03ca-4e6c-b09b-85494d20111f");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "Message",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88869cb5-6278-4773-887a-e865e9c07380", null, "user", "USER" },
                    { "f4edc403-be56-4ab8-b17e-8dc9de5e4bd5", null, "admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_userId",
                table: "Message",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_userId",
                table: "Message");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88869cb5-6278-4773-887a-e865e9c07380");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4edc403-be56-4ab8-b17e-8dc9de5e4bd5");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "Message",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                columns: new[] { "userId", "eventId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2f34a52b-62fa-468d-bbc3-7fd3e23d4e9a", null, "user", "USER" },
                    { "6b170f49-03ca-4e6c-b09b-85494d20111f", null, "admin", "ADMIN" }
                });
        }
    }
}
