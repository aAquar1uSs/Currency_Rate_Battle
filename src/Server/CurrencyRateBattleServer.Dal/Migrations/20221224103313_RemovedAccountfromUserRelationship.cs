using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Dal.Migrations
{
    public partial class RemovedAccountfromUserRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Account_AccountId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AccountId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountId",
                table: "User",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Account_AccountId",
                table: "User",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }
    }
}
