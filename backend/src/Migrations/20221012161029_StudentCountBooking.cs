using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace awl_raumreservierung.Migrations {
	public partial class StudentCountBooking : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.AddColumn<int>(
				 name: "StudentCount",
				 table: "Bookings",
				 type: "INTEGER",
				 nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropColumn(
				 name: "StudentCount",
				 table: "Bookings");
		}
	}
}
