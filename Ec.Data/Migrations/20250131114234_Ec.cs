using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ec.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5e25539d-b109-4841-9746-85d702cbc51a"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("1235c47e-85cc-4cc5-9deb-77cdf1e82a14"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEG/XQT1wjhJeFW8BGlyH1omsTWosg0+D5is60hi7SrPeuQ8BNKQX+vHg43zAvY+O0Q==", "+998945631282", 0, "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("1235c47e-85cc-4cc5-9deb-77cdf1e82a14"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("5e25539d-b109-4841-9746-85d702cbc51a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEKvEfu65Qp+5VoGXDg3p00gF10Tr4hiNqi0/+TwfZS8+gpQcEYJMfLETjTsdCKGb3A==", "+998945631282", 0, "admin", "admin" });
        }
    }
}
