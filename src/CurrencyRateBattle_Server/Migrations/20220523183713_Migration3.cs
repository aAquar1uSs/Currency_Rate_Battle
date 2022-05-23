using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("08c5fc6a-63d5-4edf-a0f2-ea715200ba45"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("4e19a30d-0778-4066-9880-221053bed5cd"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("ab84e758-3939-46cf-a45a-76d9372cf3e3"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("d748c53f-3f4c-42e8-99e5-717c00fa3678"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("f8de1def-1776-44b1-9b7b-fc6a16e4712d"));

            migrationBuilder.AddColumn<decimal>(
                name: "Payout",
                table: "Rate",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SettleDate",
                table: "Rate",
                type: "timestamp without time zone",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Payout",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "SettleDate",
                table: "Rate");

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "CurrencyName", "CurrencySymbol", "Description" },
                values: new object[,]
                {
                    { new Guid("08c5fc6a-63d5-4edf-a0f2-ea715200ba45"), "CHF", "Fr", "Swiss Franc" },
                    { new Guid("4e19a30d-0778-4066-9880-221053bed5cd"), "USD", "$", "US Dollar" },
                    { new Guid("ab84e758-3939-46cf-a45a-76d9372cf3e3"), "EUR", "$", "Euro" },
                    { new Guid("d748c53f-3f4c-42e8-99e5-717c00fa3678"), "GBP", "£", "British Pound" },
                    { new Guid("f8de1def-1776-44b1-9b7b-fc6a16e4712d"), "PLN", "zł", "Polish Zlotych" }
                });
        }
    }
}
