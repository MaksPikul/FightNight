using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fightnight.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForeignKeyFromInvitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_userEmail",
                table: "Invitation");

            migrationBuilder.DropIndex(
                name: "IX_Invitation_userEmail",
                table: "Invitation");

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
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserEmail",
                table: "Invitation",
                type: "nvarchar(256)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ab6540d-97ec-465d-920d-d60ac63dd665", null, "user", "USER" },
                    { "f4323c0a-7f37-4082-891b-791d51e50126", null, "admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_AppUserEmail",
                table: "Invitation",
                column: "AppUserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_AppUserEmail",
                table: "Invitation",
                column: "AppUserEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitation_AspNetUsers_AppUserEmail",
                table: "Invitation");

            migrationBuilder.DropIndex(
                name: "IX_Invitation_AppUserEmail",
                table: "Invitation");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ab6540d-97ec-465d-920d-d60ac63dd665");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4323c0a-7f37-4082-891b-791d51e50126");

            migrationBuilder.DropColumn(
                name: "AppUserEmail",
                table: "Invitation");

            migrationBuilder.AlterColumn<string>(
                name: "userEmail",
                table: "Invitation",
                type: "nvarchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7106afa2-f572-447d-9a98-565ab1565231", null, "admin", "ADMIN" },
                    { "be014aae-f2c8-427d-82c8-365082e54ac1", null, "user", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_userEmail",
                table: "Invitation",
                column: "userEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitation_AspNetUsers_userEmail",
                table: "Invitation",
                column: "userEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Email");
        }
    }
}
