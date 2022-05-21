using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "USDValue",
                table: "CurrencyState");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Room",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SetDate",
                table: "Rate",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "CurrencyState",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "AccountHistory",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "CurrencyName", "CurrencySymbol", "Description" },
                values: new object[,]
                {
                    { new Guid("2319edad-0070-4e90-a543-59f6b8c8e74a"), "CHF", "Fr", "Swiss Franc" },
                    { new Guid("468edf79-9f98-45d2-b913-62ad8b3def8c"), "PLN", "zł", "Polish Zlotych" },
                    { new Guid("9ec74bb4-21ca-47dc-bad7-c5c559a5f4f3"), "EUR", "$", "Euro" },
                    { new Guid("aa220d0c-1e47-449d-b20a-28a038e3879b"), "USD", "$", "US Dollar" },
                    { new Guid("dd429a00-de34-4234-bed7-8faa94f0eaa7"), "GBP", "£", "British Pound" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("2319edad-0070-4e90-a543-59f6b8c8e74a"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("468edf79-9f98-45d2-b913-62ad8b3def8c"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("9ec74bb4-21ca-47dc-bad7-c5c559a5f4f3"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("aa220d0c-1e47-449d-b20a-28a038e3879b"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("dd429a00-de34-4234-bed7-8faa94f0eaa7"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Room",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SetDate",
                table: "Rate",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "CurrencyState",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<decimal>(
                name: "USDValue",
                table: "CurrencyState",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "AccountHistory",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

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
    }
}
