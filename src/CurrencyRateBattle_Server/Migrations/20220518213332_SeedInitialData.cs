using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "CurrencyName", "CurrencySymbol", "Description" },
                values: new object[,]
                {
                    { new Guid("6a7c8c7b-549e-454b-9676-8e6dde8c7ed9"), "GBP", "£", "British Pound" },
                    { new Guid("8076a7b0-88cb-4ac9-944b-f322877741fb"), "USD", "$", "US Dollar" },
                    { new Guid("93138db7-9b1a-401f-9ca4-f0637c53faff"), "CHF", "Fr", "Swiss Franc" },
                    { new Guid("afd312dc-f3d8-4269-bb33-377658aace8d"), "PLN", "zł", "Polish Zlotych" },
                    { new Guid("f47e6037-92ab-43ae-9e53-8f3063dc2f4b"), "EUR", "$", "Euro" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("6a7c8c7b-549e-454b-9676-8e6dde8c7ed9"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("8076a7b0-88cb-4ac9-944b-f322877741fb"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("93138db7-9b1a-401f-9ca4-f0637c53faff"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("afd312dc-f3d8-4269-bb33-377658aace8d"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("f47e6037-92ab-43ae-9e53-8f3063dc2f4b"));
        }
    }
}
