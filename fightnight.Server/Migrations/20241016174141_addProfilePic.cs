using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class addProfilePic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ab6540d-97ec-465d-920d-d60ac63dd665");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4323c0a-7f37-4082-891b-791d51e50126");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f890456-6ef5-4884-ba48-07eded1dbcf3", null, "admin", "ADMIN" },
                    { "5a899a49-786f-4d73-99e7-55610108f3e4", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f890456-6ef5-4884-ba48-07eded1dbcf3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a899a49-786f-4d73-99e7-55610108f3e4");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ab6540d-97ec-465d-920d-d60ac63dd665", null, "user", "USER" },
                    { "f4323c0a-7f37-4082-891b-791d51e50126", null, "admin", "ADMIN" }
                });
        }
    }
}
