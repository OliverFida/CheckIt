using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace awl_raumreservierung.Migrations {
	public partial class Init : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.CreateTable(
				 name: "Bookings",
				 columns: table => new {
					 Id = table.Column<long>(type: "INTEGER", nullable: false)
							.Annotation("Sqlite:Autoincrement", true),
					 StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
					 EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
					 Room = table.Column<long>(type: "INTEGER", nullable: false),
					 UserId = table.Column<long>(type: "INTEGER", nullable: false),
					 CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
					 CreatedBy = table.Column<long>(type: "INTEGER", nullable: true),
					 Note = table.Column<string>(type: "TEXT", nullable: true)
				 },
				 constraints: table => table.PrimaryKey("PK_Bookings", x => x.Id));

			migrationBuilder.CreateTable(
				 name: "Rooms",
				 columns: table => new {
					 Id = table.Column<long>(type: "INTEGER", nullable: false)
							.Annotation("Sqlite:Autoincrement", true),
					 Number = table.Column<string>(type: "TEXT", nullable: true),
					 Name = table.Column<string>(type: "TEXT", nullable: true),
					 Active = table.Column<bool>(type: "INTEGER", nullable: false)
				 },
				 constraints: table => table.PrimaryKey("PK_Rooms", x => x.Id));

			migrationBuilder.CreateTable(
				 name: "Users",
				 columns: table => new {
					 Id = table.Column<long>(type: "INTEGER", nullable: false)
							.Annotation("Sqlite:Autoincrement", true),
					 Username = table.Column<string>(type: "TEXT", nullable: false),
					 Firstname = table.Column<string>(type: "TEXT", nullable: false),
					 Lastname = table.Column<string>(type: "TEXT", nullable: false),
					 Passwd = table.Column<string>(type: "TEXT", nullable: false),
					 Lastlogon = table.Column<DateTime>(type: "TEXT", nullable: true),
					 Lastchange = table.Column<DateTime>(type: "TEXT", nullable: true),
					 Active = table.Column<bool>(type: "INTEGER", nullable: false),
					 Role = table.Column<int>(type: "INTEGER", nullable: false)
				 },
				 constraints: table => table.PrimaryKey("PK_Users", x => x.Id));
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
				 name: "Bookings");

			migrationBuilder.DropTable(
				 name: "Rooms");

			migrationBuilder.DropTable(
				 name: "Users");
		}
	}
}
