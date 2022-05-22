using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHistory_Room_RoomId",
                table: "AccountHistory");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "AccountHistory",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AccountHistory_Room_RoomId",
                table: "AccountHistory",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHistory_Room_RoomId",
                table: "AccountHistory");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "AccountHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_AccountHistory_Room_RoomId",
                table: "AccountHistory",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
