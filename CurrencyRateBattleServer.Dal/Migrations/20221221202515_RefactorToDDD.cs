using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Dal.Migrations
{
    public partial class RefactorToDDD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyName = table.Column<string>(type: "varchar(3)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "varchar(3)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal", nullable: false),
                    Description = table.Column<string>(type: "varchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyName);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyState",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CurrencyExchangeRate = table.Column<decimal>(type: "numeric", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyName = table.Column<string>(type: "text", nullable: false),
                    CurrencyCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyState_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    IsCredit = table.Column<bool>(type: "boolean", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountHistory_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountHistory_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SetDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RateCurrencyExchange = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    SettleDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Payout = table.Column<decimal>(type: "numeric", nullable: true),
                    IsClosed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsWon = table.Column<bool>(type: "boolean", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyName = table.Column<string>(type: "varchar(3)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rate_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rate_Currency_CurrencyName",
                        column: x => x.CurrencyName,
                        principalTable: "Currency",
                        principalColumn: "CurrencyName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rate_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "CurrencyName", "CurrencyCode", "Description", "Rate" },
                values: new object[,]
                {
                    { "CHF", "Fr", "Swiss Franc", 0m },
                    { "EUR", "$", "Euro", 0m },
                    { "GBP", "£", "British Pound", 0m },
                    { "PLN", "zł", "Polish Zlotych", 0m },
                    { "USD", "$", "US Dollar", 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountHistory_AccountId",
                table: "AccountHistory",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountHistory_RoomId",
                table: "AccountHistory",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyState_RoomId_CurrencyCode",
                table: "CurrencyState",
                columns: new[] { "RoomId", "CurrencyCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rate_AccountId",
                table: "Rate",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_CurrencyName",
                table: "Rate",
                column: "CurrencyName");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_RoomId",
                table: "Rate",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountId",
                table: "User",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_User_UserId",
                table: "Account",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_User_UserId",
                table: "Account");

            migrationBuilder.DropTable(
                name: "AccountHistory");

            migrationBuilder.DropTable(
                name: "CurrencyState");

            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
