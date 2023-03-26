using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyRateBattleServer.Dal.Migrations
{
    public partial class RefactorDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Currency",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 5, 11, 49, 8, 856, DateTimeKind.Utc).AddTicks(5113),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Currency",
                type: "decimal",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateDate",
                table: "Currency",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2023, 3, 5, 11, 49, 8, 856, DateTimeKind.Utc).AddTicks(5113));

            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Currency",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal",
                oldDefaultValue: 0m);
        }
    }
}
