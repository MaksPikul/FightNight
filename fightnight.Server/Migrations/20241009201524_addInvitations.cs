using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class addInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88869cb5-6278-4773-887a-e865e9c07380");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4edc403-be56-4ab8-b17e-8dc9de5e4bd5");

            migrationBuilder.RenameColumn(
                name: "adminId",
                table: "Event",
                newName: "joinCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Event",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updatedAt",
                table: "Event",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Invitation",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    eventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    invitedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    respondedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    proposedRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitation_AspNetUsers_userEmail",
                        column: x => x.userEmail,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitation_Event_eventId",
                        column: x => x.eventId,
                        principalTable: "Event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2db271c4-62d5-4ba3-9339-7d7e380d9930", null, "admin", "ADMIN" },
                    { "302e8bab-5a6b-4329-ae53-1e2dba55c59f", null, "user", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_eventId",
                table: "Invitation",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_userEmail",
                table: "Invitation",
                column: "userEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitation");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2db271c4-62d5-4ba3-9339-7d7e380d9930");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "302e8bab-5a6b-4329-ae53-1e2dba55c59f");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Event");

            migrationBuilder.RenameColumn(
                name: "joinCode",
                table: "Event",
                newName: "adminId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88869cb5-6278-4773-887a-e865e9c07380", null, "user", "USER" },
                    { "f4edc403-be56-4ab8-b17e-8dc9de5e4bd5", null, "admin", "ADMIN" }
                });
        }
    }
}
