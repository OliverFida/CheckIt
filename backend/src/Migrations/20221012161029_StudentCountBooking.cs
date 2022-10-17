using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace awl_raumreservierung.Migrations {
	/// <summary>
	/// 
	/// </summary>
	public partial class StudentCountBooking : Migration {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="migrationBuilder"></param>
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.AddColumn<int>(
				 name: "StudentCount",
				 table: "Bookings",
				 type: "INTEGER",
				 nullable: true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="migrationBuilder"></param>
		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropColumn(
				 name: "StudentCount",
				 table: "Bookings");
		}
	}
}
