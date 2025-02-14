using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ec.Data.Migrations
{
    /// <inheritdoc />
    public partial class EC5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("43e7931b-fc62-4655-b6f6-c68decd5b891"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Statistics",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsFlaggedForReview",
                table: "Statistics",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousEarnings",
                table: "Statistics",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousSoldCount",
                table: "Statistics",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ReviewNotes",
                table: "Statistics",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Statistics",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "StatisticHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StatisticId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductCount = table.Column<int>(type: "integer", nullable: false),
                    SoldCount = table.Column<int>(type: "integer", nullable: false),
                    Earnings = table.Column<decimal>(type: "numeric", nullable: false),
                    Period = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticHistory_Statistics_StatisticId",
                        column: x => x.StatisticId,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("09a59fad-4297-4aa4-9418-62329c5d9688"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEHxxDGdYk8TfOjb4ItiCAPqj0b0AnrzqZhBh7kQz1JddVrxorRgoJsRTiIPef64g6A==", "+998945631282", 0, "admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticHistory_StatisticId",
                table: "StatisticHistory",
                column: "StatisticId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticHistory");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("09a59fad-4297-4aa4-9418-62329c5d9688"));

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "IsFlaggedForReview",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "PreviousEarnings",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "PreviousSoldCount",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "ReviewNotes",
                table: "Statistics");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Statistics");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "FullName", "IsBlocked", "PasswordHash", "PhoneNumber", "Rank", "Role", "Username" },
                values: new object[] { new Guid("43e7931b-fc62-4655-b6f6-c68decd5b891"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shermatov Asadbek", false, "AQAAAAIAAYagAAAAEPCGVdq7T4GKLDPTbPWRUqgo4P5PUDECpA1pzvIdjhH6dK1P9KDvcMSpSnG5i0sjew==", "+998945631282", 0, "admin", "admin" });
        }
    }
}
