using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace awl_raumreservierung.Migrations
{
    public partial class AdminPWMD5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Passwd",
                value: "$2a$11$CC3I/ZPjbKbckPma1jgZV.VFwsFu1hxY0zaQAPFA0N22O4EY7bABC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Passwd",
                value: "$2a$11$YfH.ZTiaAw36LLbXxTTDs.yVIreaGcEZ9lQsIjwAotpuCBPY7GVyW");
        }
    }
}
