using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace awl_raumreservierung.Migrations {
	public partial class PWHash : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.InsertData(
				 table: "Users",
				 columns: new[] { "Id", "Active", "Firstname", "Lastchange", "Lastlogon", "Lastname", "Passwd", "Role", "Username" },
				 values: new object[] { 1L, true, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Benutzer", "$2a$11$YfH.ZTiaAw36LLbXxTTDs.yVIreaGcEZ9lQsIjwAotpuCBPY7GVyW", 1, "admin" });
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DeleteData(
				 table: "Users",
				 keyColumn: "Id",
				 keyValue: 1L);
		}
	}
}
