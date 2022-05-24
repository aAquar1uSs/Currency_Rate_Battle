using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Migrations
{
    public partial class Migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("0f49f4f4-62f2-4e9a-babc-f4f944b511ba"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("3dca2505-c97a-4fe0-a586-bf9cdc7a83a8"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("729f0615-a1f3-4bc4-a917-abca615149ee"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("9638cc7d-48e5-44ef-8a6e-da9cc1c3afaa"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("c3bda91d-816d-4d7e-b3a3-1d828f548fa4"));

            migrationBuilder.AddColumn<decimal>(
                name: "RateCurrencyExchange",
                table: "Rate",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "CurrencyName", "CurrencySymbol", "Description" },
                values: new object[,]
                {
                    { new Guid("1b510d3b-b21c-4e3e-a30d-82e1fd9f48b5"), "USD", "$", "US Dollar" },
                    { new Guid("8a9c0d2e-8fd2-4896-aaa6-140847537cba"), "PLN", "zł", "Polish Zlotych" },
                    { new Guid("9ecf312b-fa37-4afb-ad21-44175b9d8e8c"), "CHF", "Fr", "Swiss Franc" },
                    { new Guid("bf281a21-46d7-4791-929b-d6d70f6cf011"), "GBP", "£", "British Pound" },
                    { new Guid("cf2c0721-3954-4b31-aa9f-bf1d476e3eed"), "EUR", "$", "Euro" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("1b510d3b-b21c-4e3e-a30d-82e1fd9f48b5"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("8a9c0d2e-8fd2-4896-aaa6-140847537cba"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("9ecf312b-fa37-4afb-ad21-44175b9d8e8c"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("bf281a21-46d7-4791-929b-d6d70f6cf011"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("cf2c0721-3954-4b31-aa9f-bf1d476e3eed"));

            migrationBuilder.DropColumn(
                name: "RateCurrencyExchange",
                table: "Rate");

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "CurrencyName", "CurrencySymbol", "Description" },
                values: new object[,]
                {
                    { new Guid("0f49f4f4-62f2-4e9a-babc-f4f944b511ba"), "PLN", "zł", "Polish Zlotych" },
                    { new Guid("3dca2505-c97a-4fe0-a586-bf9cdc7a83a8"), "USD", "$", "US Dollar" },
                    { new Guid("729f0615-a1f3-4bc4-a917-abca615149ee"), "GBP", "£", "British Pound" },
                    { new Guid("9638cc7d-48e5-44ef-8a6e-da9cc1c3afaa"), "CHF", "Fr", "Swiss Franc" },
                    { new Guid("c3bda91d-816d-4d7e-b3a3-1d828f548fa4"), "EUR", "$", "Euro" }
                });
        }
    }
}
