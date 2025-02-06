using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ec.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ec2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("46aea257-f375-4e75-99da-1967566a06d5"));

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Feedbacks",
                newName: "Rank");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Products",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 500);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("a2309ad1-cd43-4b1b-af05-d7ebe7343af8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEAwQ2P+PPtnDmpidZ46X9Vb3lPqJBJpK1E7WB8hIUlzl0mwVRYp3P8eDOPuEkwzfXw==", "+998945631282", 0, "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a2309ad1-cd43-4b1b-af05-d7ebe7343af8"));

            migrationBuilder.RenameColumn(
                name: "Rank",
                table: "Feedbacks",
                newName: "Rating");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Products",
                type: "integer",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("46aea257-f375-4e75-99da-1967566a06d5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEPN+PPM5cHLY57w4Kv+gL8QviFKoPSBld3Gr8az/1J+/d0MekmHxhqYtn6nj/uoiSw==", "+998945631282", 0, "admin", "admin" });
        }
    }
}
