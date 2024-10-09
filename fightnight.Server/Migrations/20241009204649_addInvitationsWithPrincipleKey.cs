using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class addInvitationsWithPrincipleKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_userEmail",
                table: "Invitation");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2db271c4-62d5-4ba3-9339-7d7e380d9930");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "302e8bab-5a6b-4329-ae53-1e2dba55c59f");

            migrationBuilder.AlterColumn<string>(
                name: "userEmail",
                table: "Invitation",
                type: "nvarchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7106afa2-f572-447d-9a98-565ab1565231", null, "admin", "ADMIN" },
                    { "be014aae-f2c8-427d-82c8-365082e54ac1", null, "user", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_userEmail",
                table: "Invitation",
                column: "userEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_userEmail",
                table: "Invitation");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7106afa2-f572-447d-9a98-565ab1565231");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be014aae-f2c8-427d-82c8-365082e54ac1");

            migrationBuilder.AlterColumn<string>(
                name: "userEmail",
                table: "Invitation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2db271c4-62d5-4ba3-9339-7d7e380d9930", null, "admin", "ADMIN" },
                    { "302e8bab-5a6b-4329-ae53-1e2dba55c59f", null, "user", "USER" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_userEmail",
                table: "Invitation",
                column: "userEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
