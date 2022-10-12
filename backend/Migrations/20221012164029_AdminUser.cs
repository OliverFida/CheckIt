using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace awl_raumreservierung.Migrations
{
    public partial class AdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Active", "Lastchange" },
                values: new object[] { true, new DateTime(2022, 10, 12, 16, 40, 29, 222, DateTimeKind.Utc).AddTicks(5735) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Active", "Lastchange" },
                values: new object[] { false, new DateTime(2022, 10, 12, 16, 38, 22, 631, DateTimeKind.Utc).AddTicks(9620) });
        }
    }
}
