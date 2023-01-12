using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Dal.Migrations
{
    public partial class refactorDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Currency_CurrencyCode",
                table: "Rate");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Rate",
                newName: "CurrencyName");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_CurrencyCode",
                table: "Rate",
                newName: "IX_Rate_CurrencyName");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Currency_CurrencyName",
                table: "Rate",
                column: "CurrencyName",
                principalTable: "Currency",
                principalColumn: "CurrencyName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rate_Currency_CurrencyName",
                table: "Rate");

            migrationBuilder.RenameColumn(
                name: "CurrencyName",
                table: "Rate",
                newName: "CurrencyCode");

            migrationBuilder.RenameIndex(
                name: "IX_Rate_CurrencyName",
                table: "Rate",
                newName: "IX_Rate_CurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Rate_Currency_CurrencyCode",
                table: "Rate",
                column: "CurrencyCode",
                principalTable: "Currency",
                principalColumn: "CurrencyName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
