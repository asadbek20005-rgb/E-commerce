using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ec.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ec3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1235c47e-85cc-4cc5-9deb-77cdf1e82a14"));

            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "ProductContent",
                newName: "FileUrl");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ProductContent",
                newName: "FileType");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Messages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("c563d7ae-0572-4924-b0b7-f03d91dc2dba"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEFAGZl0frcI1wkOcp5DqEPGSUIF7MTkn15i9LTH9PNw4gzaxJOQn5RGM2X2os7RLRA==", "+998945631282", 0, "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c563d7ae-0572-4924-b0b7-f03d91dc2dba"));

            migrationBuilder.RenameColumn(
                name: "FileUrl",
                table: "ProductContent",
                newName: "VideoUrl");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "ProductContent",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("1235c47e-85cc-4cc5-9deb-77cdf1e82a14"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEG/XQT1wjhJeFW8BGlyH1omsTWosg0+D5is60hi7SrPeuQ8BNKQX+vHg43zAvY+O0Q==", "+998945631282", 0, "admin", "admin" });
        }
    }
}
