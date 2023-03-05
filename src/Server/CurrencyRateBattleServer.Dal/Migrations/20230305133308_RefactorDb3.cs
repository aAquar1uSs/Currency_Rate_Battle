using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Dal.Migrations
{
    public partial class RefactorDb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_User_UserId",
                table: "Account");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Account_UserId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Account");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Currency",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 5, 13, 33, 8, 334, DateTimeKind.Utc).AddTicks(2752),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 3, 5, 11, 49, 8, 856, DateTimeKind.Utc).AddTicks(5113));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_User_Email",
                table: "Account",
                column: "Email",
                principalTable: "User",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_User_Email",
                table: "Account");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Account_Email",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Account");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "User",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Currency",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 5, 11, 49, 8, 856, DateTimeKind.Utc).AddTicks(5113),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 3, 5, 13, 33, 8, 334, DateTimeKind.Utc).AddTicks(2752));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Account",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_User_UserId",
                table: "Account",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
