namespace awl_raumreservierung.core {
	/// <summary>
	/// Stellt Helfermothoden bereit
	/// </summary>
	public class Helpers {
		private readonly checkITContext ctx;

		/// <summary>
		/// Erstellt eine neue Helper-Classe mit DB-Context
		/// </summary>
		/// <param name="context">DB-Context</param>
		public Helpers(checkITContext context) => ctx = context;

		/// <summary>
		/// Holt einen User aus der Datenbank
		/// </summary>
		/// <param name="username">Username</param>
		/// <returns></returns>
		public User GetUser(string username) {
			var user = ctx.Users.Where(u => u.Username.ToLower().Trim() == username.ToLower().Trim()).FirstOrDefault();

			return user is null ? throw new Exception($"Zum Namen {username} wurde kein Benutzer gefunden.") : user;
		}

		/// <summary>
		/// Checkt, ob ein Nutzer existiert
		/// </summary>
		/// <param name="username">Benutzername</param>
		/// <returns></returns>
		public bool DoesUserExist(string username) => ctx.Users.Any(u => u.Username.ToLower() == username.ToLower());

		/// <summary>
		/// Checkt, ob ein Nutzer existiert
		/// </summary>
		/// <param name="userID">User-ID</param>
		/// <returns></returns>
		public bool DoesUserExist(int userID) => ctx.Users.Find(userID) is not null;

		/// <summary>
		/// Holt einen User aus der Datenbank
		/// </summary>
		/// <param name="id">User-ID</param>
		/// <returns></returns>
		public User GetUser(int id) {
			var user = ctx.Users.Where(u => u.Id == id).FirstOrDefault();

			return user is null ? throw new Exception($"Zur ID {id} wurde kein Benutzer gefunden.") : user;
		}

		/// <summary>
		/// Holt einen Raum aus der DB
		/// </summary>
		/// <param name="id">ID des Raums</param>
		/// <returns></returns>
		public Room GetRoom(long id) {
			var room = ctx.Rooms.Where(r => r.Id == id).FirstOrDefault();

			return room is null ? throw new Exception($"Zur ID {id} wurde kein Raum gefunden.") : room;
		}

		/// <summary>
		/// Checkt, ob ein Raum existiert
		/// </summary>
		/// <param name="id">ID des Raums</param>
		/// <returns></returns>
		public bool DoesRoomExist(long id) => ctx.Rooms.Find(id) is not null;

		/// <summary>
		/// Holt ein Booking aus der DB
		/// </summary>
		/// <param name="id">Booking-ID</param>
		/// <returns></returns>
		public Booking GetBooking(long id) {
			var booking = ctx.Bookings.Where(b => b.Id == id).FirstOrDefault();

			return booking is null ? throw new Exception($"Zur ID {id} wurde keine Buchung gefunden.") : booking;
		}

		/// <summary>
		/// Checkt, ob ein spezifisches Booking existiert
		/// </summary>
		/// <param name="id">ID des Bookings</param>
		/// <returns></returns>
		public bool DoesBookingExist(long id) => ctx.Bookings.Find(id) is not null;

		/// <summary>
		/// Checkt, ob ein Booking sich mit einem anderen überschneidet
		/// </summary>
		/// <param name="model">Bookingmodel</param>
		/// <param name="ctx">Bookingmodel</param>
		/// <returns></returns>
		public bool BookingOverlaps(CreateBookingModel model, checkITContext ctx) {
			var room = GetRoom(model.RoomID);

			var overlapsWithOtherBookings = room.GetBookings(ctx).Any(b => b.StartTime <= model.EndTime.Subtract(new TimeSpan(0, 0, 1)) && model.StartTime <= b.EndTime);
			return overlapsWithOtherBookings;
		}

		/// <summary>
		/// Checkt, ob ein Booking sich mit einem anderen überschneidet
		/// </summary>
		/// <param name="booking">Booking</param>
		/// <param name="ctx">Booking</param>
		/// <returns></returns>
		public bool BookingOverlaps(Booking booking, checkITContext ctx) {
			var room = GetRoom(booking.Room);

			var overlapsWithOtherBookings = room.GetBookings(ctx).Any(b => b != booking && b.StartTime <= booking.EndTime.Subtract(new TimeSpan(0, 0, 1)) && booking.StartTime <= b.EndTime);
			return overlapsWithOtherBookings;
		}

		/// <summary>
		/// Checkt ob die Zeiten einer Buchung valide sind.
		/// </summary>
		/// <param name="booking">Booking</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public bool CheckBookingTime(Booking booking) {
			return booking.StartTime > booking.EndTime
				? throw new ArgumentException("Endzeit vor Startzeit")
				: BookingOverlaps(booking, ctx) ? throw new ArgumentException("Buchung überlappt mit anderer Buchung") : true;
		}

		/// <summary>
		/// Setzt den Aktivstatus eines Users
		/// </summary>
		/// <param name="user">User</param>
		/// <param name="status">Status</param>
		public void SetUserStatus(User user, bool status) {
			user.Active = status;
			user.Lastchange = DateTime.Now.ToUniversalTime();
			_ = ctx.Users.Update(user);
			_ = ctx.SaveChanges();
		}
	}
}
