using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class imgToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0f4d3c29-55ce-4b47-9635-c70739b89403");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "186aebd7-c7a3-49b2-b825-5f79e35c498d");

            migrationBuilder.RenameColumn(
                name: "VenueAddress",
                table: "Event",
                newName: "venueAddress");

            migrationBuilder.AddColumn<string>(
                name: "bannerUrl",
                table: "Event",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    appUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_AspNetUsers_appUserId",
                        column: x => x.appUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "376ab579-d1fc-4909-beaa-d646db87a4b1", null, "admin", "ADMIN" },
                    { "c66e9867-37e9-4722-89a3-7b02c669ab46", null, "user", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_appUserId",
                table: "UserToken",
                column: "appUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "376ab579-d1fc-4909-beaa-d646db87a4b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c66e9867-37e9-4722-89a3-7b02c669ab46");

            migrationBuilder.DropColumn(
                name: "bannerUrl",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "venueAddress",
                table: "Event",
                newName: "VenueAddress");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0f4d3c29-55ce-4b47-9635-c70739b89403", null, "user", "USER" },
                    { "186aebd7-c7a3-49b2-b825-5f79e35c498d", null, "admin", "ADMIN" }
                });
        }
    }
}
