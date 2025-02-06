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
                keyValue: new Guid("a2309ad1-cd43-4b1b-af05-d7ebe7343af8"));

            migrationBuilder.AlterColumn<string>(
                name: "Longitude",
                table: "Address",
                type: "text",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Latitude",
                table: "Address",
                type: "text",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("43e7931b-fc62-4655-b6f6-c68decd5b891"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEPCGVdq7T4GKLDPTbPWRUqgo4P5PUDECpA1pzvIdjhH6dK1P9KDvcMSpSnG5i0sjew==", "+998945631282", 0, "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("43e7931b-fc62-4655-b6f6-c68decd5b891"));

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "Address",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Address",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("a2309ad1-cd43-4b1b-af05-d7ebe7343af8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEAwQ2P+PPtnDmpidZ46X9Vb3lPqJBJpK1E7WB8hIUlzl0mwVRYp3P8eDOPuEkwzfXw==", "+998945631282", 0, "admin", "admin" });
        }
    }
}
