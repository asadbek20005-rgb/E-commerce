using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ec.Data.Migrations
{
    /// <inheritdoc />
    public partial class ec3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Chats_ChatId",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f5744648-4352-4496-bd4b-192b68dc8648"));

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ChatId",
                table: "Messages",
                newName: "IX_Messages_ChatId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "OTPs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "OTPs",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("3ba5fb3e-422e-4a1d-bc22-acf2ab577f12"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, null, "+998945631282", 0, "super admin", "spawn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3ba5fb3e-422e-4a1d-bc22-acf2ab577f12"));

            migrationBuilder.DropColumn(
                name: "Username",
                table: "OTPs");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatId",
                table: "Message",
                newName: "IX_Message_ChatId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "OTPs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("f5744648-4352-4496-bd4b-192b68dc8648"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, null, "+998945631282", 0, "super admin", "spawn" });

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Chats_ChatId",
                table: "Message",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
