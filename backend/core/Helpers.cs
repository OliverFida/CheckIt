using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung
{
	/// <summary>
	/// Stellt Helfermothoden bereit
	/// </summary>
	public class Helpers
	{
		/// <summary>
		/// Holt einen User aus der Datenbank
		/// </summary>
		/// <param name="username">Username</param>
		/// <returns></returns>
		public static User GetUser(string username)
		{
			using checkITContext ctx = new();
			var user = ctx.Users.Where(u => u.Username.ToLower().Trim() == username.ToLower().Trim()).FirstOrDefault();

			if (user is null)
			{
				throw new Exception($"Zum Namen {username} wurde kein Benutzer gefunden.");
			}

			return user;
		}

		/// <summary>
		/// Checkt, ob ein Nutzer existiert
		/// </summary>
		/// <param name="username">Benutzername</param>
		/// <returns></returns>
		public static bool DoesUserExist(string username)
		{
			return new checkITContext().Users.Any(u => u.Username == username);
		}

		/// <summary>
		/// Checkt, ob ein Nutzer existiert
		/// </summary>
		/// <param name="userID">User-ID</param>
		/// <returns></returns>
		public static bool DoesUserExist(int userID)
		{
			return new checkITContext().Users.Find(userID) is null;
		}

		/// <summary>
		/// Holt einen User aus der Datenbank
		/// </summary>
		/// <param name="id">User-ID</param>
		/// <returns></returns>
		public static User GetUser(int id)
		{
			using checkITContext ctx = new();
			var user = ctx.Users.Where(u => u.Id == id).FirstOrDefault();

			if (user is null)
			{
				throw new Exception($"Zur ID {id} wurde kein Benutzer gefunden.");
			}

			return user;
		}

		/// <summary>
		/// Holt einen Raum aus der DB
		/// </summary>
		/// <param name="id">ID des Raums</param>
		/// <returns></returns>
		public static Room GetRoom(long id)
		{
			using checkITContext ctx = new();
			var room = ctx.Rooms.Where(r => r.Id == id).FirstOrDefault();

			if (room is null)
			{
				throw new Exception($"Zur ID {id} wurde kein Raum gefunden.");
			}

			return room;
		}

		/// <summary>
		/// Holt ein Booking aus der DB
		/// </summary>
		/// <param name="id">Booking-ID</param>
		/// <returns></returns>
		public static Booking GetBooking(long id)
		{
			using checkITContext ctx = new();
			var booking = ctx.Bookings.Where(b => b.Id == id).FirstOrDefault();

			if (booking is null)
			{
				throw new Exception($"Zur ID {id} wurde keine Buchung gefunden.");
			}

			return booking;
		}

		/// <summary>
		/// Checkt, ob ein Booking sich mit einem anderen überschneidet
		/// </summary>
		/// <param name="model">Bookingmodel</param>
		/// <returns></returns>
		public static bool BookingOverlaps(CreateBookingModel model)
		{
			var room = GetRoom(model.RoomID);

			bool overlapsWithOtherBookings = room.GetBookings().Any(b => b.StartTime <= model.EndTime.Subtract(new TimeSpan(0, 0, 1)) && model.StartTime <= b.EndTime);
			return overlapsWithOtherBookings;
		}

		/// <summary>
		/// Checkt, ob ein Booking sich mit einem anderen überschneidet
		/// </summary>
		/// <param name="booking">Booking</param>
		/// <returns></returns>
		public static bool BookingOverlaps(Booking booking)
		{
			var room = GetRoom(booking.Id);

			bool overlapsWithOtherBookings = room.GetBookings().Any(b => b.StartTime <= booking.EndTime.Subtract(new TimeSpan(0, 0, 1)) && booking.StartTime <= b.EndTime);
			return overlapsWithOtherBookings;
		}

		/// <summary>
		/// Checkt ob die Zeiten einer Buchung valide sind.
		/// </summary>
		/// <param name="booking">Booking</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static bool checkBookingTime(Booking booking)
		{
			if (booking.StartTime > booking.EndTime)
			{
				throw new ArgumentException("Endzeit vor Startzeit");
			}
			if (BookingOverlaps(booking))
			{
				throw new ArgumentException("Buchung überlappt mit anderer Buchung");
			}
			return true;
		}

		/// <summary>
		/// Setzt den Aktivstatus eines Users
		/// </summary>
		/// <param name="user">User</param>
		/// <param name="status">Status</param>
		public static void SetUserStatus(User user, bool status)
		{
			var ctx = new checkITContext();

			user.Active = status;
			user.Lastchange = DateTime.Now;
			ctx.Users.Update(user);
			ctx.SaveChanges();
		}
	}
}
