using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace awl_raumreservierung.core {
	/// <summary>
	/// Stellt Extensions bereit
	/// </summary>
	public static class ExtensionMethods {
		/// <summary>
		/// Checkt, ob ein String Null oder Whitespace ist
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsNullOrWhiteSpace(this string? str) => string.IsNullOrWhiteSpace(str);

		/// <summary>
		/// Konvertiert eine Zeit in UTC
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static DateTime? ToUtc(this DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.ToUniversalTime() : dateTime;

		/// <summary>
		/// Castet Long zu Int
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static int ToInt(this long num) {
			try {
				return Convert.ToInt32(num);
			} catch {
				return 0;
			}
		}

		/// <summary>
		/// Erzeugt einen PublicUser aus einem USer
		/// </summary>
		/// <param name="usr"></param>
		/// <returns></returns>
		public static PublicUser ToPublicUser(this User usr) => new(usr);

		/// <summary>
		/// Erzeugt einen PublicRoom aus einem Room
		/// </summary>
		/// <param name="room"></param>
		/// <returns></returns>
		public static PublicRoom ToPublicRoom(this Room room) => new(room);

		/// <summary>
		/// Erzeugt ein PublicBooking aus einem Booking
		/// </summary>
		/// <param name="booking"></param>
		/// <param name="helper"></param>
		/// <returns></returns>
		public static PublicBooking ToPublicBooking(this Booking booking, Helpers helper) => new(booking, helper);

		/// <summary>
		/// Liefert den Starttag der Woche
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateTime StartOfWeek(this DateTime dt) {
			var diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
			return dt.AddDays(-1 * diff).Date;
		}

		/// <summary>
		/// Liefert das Enddatum der nächsten Woche
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static DateTime EndOfNextWeek(this DateTime dt) {
			var diff = DayOfWeek.Friday - dt.DayOfWeek + 7;
			return dt.AddDays(diff);
		}

		/// <summary>
		/// Gibt den User des Auth-Users zurück
		/// </summary>
		/// <param name="princ"></param>
		/// <param name="helper"></param>
		/// <returns></returns>
		public static User GetUser(this ClaimsPrincipal princ, Helpers helper) {
			var username = princ.FindFirstValue(ClaimTypes.NameIdentifier); // Note: Nicht Single line weil er sonst bs macht
			return helper.GetUser(username);
		}

		/// <summary>
		/// Gibt den Username des Auth-Users zurück
		/// </summary>
		/// <param name="princ"></param>
		/// <returns></returns>
		public static string GetUsername(this ClaimsPrincipal princ) {
			var username = princ.FindFirstValue(ClaimTypes.NameIdentifier); // Note: Nicht Single line weil er sonst bs macht
			return username;
		}

		/// <summary>
		/// Checkt, ob der User in einer Rolle ist
		/// </summary>
		/// <param name="princ"></param>
		/// <param name="roleName">Name der Rolle</param>
		/// <returns></returns>
		public static bool IsInRole(this ClaimsPrincipal princ, string roleName) => princ.FindAll(ClaimTypes.Role).Any(c => c.Type == ClaimTypes.Role && c.Value == roleName);

		/// <summary>
		/// /// Holt alle Bookings eines Raumes
		/// </summary>
		/// <param name="room"></param>
		/// <param name="ctx"></param>
		/// <returns></returns>
		public static IEnumerable<Booking> GetBookings(this Room room, checkITContext ctx) {
			try {
				return ctx.Bookings.Where(b => b.Room == room.Id).ToArray();
			} catch {
				return new List<Booking>().ToArray();
			}
		}

		/// <summary>
		/// Erstellt ein Fehlerreturnmodel
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="ex">Exception</param>
		/// <returns></returns>
		public static ReturnModel GetErrorModel(this ControllerBase controller, Exception ex) {
			Debug.WriteLine("Fehler aufgetreten: ", ex);

			controller.Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel() {
				Status = 400,
				Message = $"{ex}"
			};
		}
	}
}
