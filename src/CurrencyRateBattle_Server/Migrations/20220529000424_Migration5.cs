using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Migrations
{
    public partial class Migration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CurrencyState_RoomId",
                table: "CurrencyState");

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

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "CurrencyName", "CurrencySymbol", "Description" },
                values: new object[,]
                {
                    { new Guid("06c95e5b-b7ed-432f-b65c-58699360b9fa"), "PLN", "zł", "Polish Zlotych" },
                    { new Guid("544f135f-fb08-4bfc-87da-883f02c1752c"), "USD", "$", "US Dollar" },
                    { new Guid("d05b2287-6e14-417f-ac34-ca61100890f9"), "EUR", "$", "Euro" },
                    { new Guid("d137d3c2-aec6-424e-99b9-ff38c7a38a8a"), "GBP", "£", "British Pound" },
                    { new Guid("f565159c-ae5e-47a3-a25b-02d11a4aadd5"), "CHF", "Fr", "Swiss Franc" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyState_RoomId_CurrencyId",
                table: "CurrencyState",
                columns: new[] { "RoomId", "CurrencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currency_CurrencyName",
                table: "Currency",
                column: "CurrencyName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_CurrencyState_RoomId_CurrencyId",
                table: "CurrencyState");

            migrationBuilder.DropIndex(
                name: "IX_Currency_CurrencyName",
                table: "Currency");

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("06c95e5b-b7ed-432f-b65c-58699360b9fa"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("544f135f-fb08-4bfc-87da-883f02c1752c"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("d05b2287-6e14-417f-ac34-ca61100890f9"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("d137d3c2-aec6-424e-99b9-ff38c7a38a8a"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("f565159c-ae5e-47a3-a25b-02d11a4aadd5"));

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

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyState_RoomId",
                table: "CurrencyState",
                column: "RoomId");
        }
    }
}
