using System.Security.Claims;

namespace awl_raumreservierung
{

    public static class ExtensionMethods
    {

        public static bool IsNullOrWhiteSpace(this string? str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static DateTime? ToUtc(this DateTime? dateTime)
        {
            if(dateTime.HasValue)
            {
                return dateTime.Value.ToUniversalTime();
            }

            return dateTime;
        }

        public static int ToInt(this long num)
        {
            try
            {
                return Convert.ToInt32(num);
            }
            catch
            {
                return 0;
            }
        }

        public static PublicUser ToPublicUser(this User usr)
        {
            return new PublicUser(usr);
        }

        public static PublicRoom ToPublicRoom(this Room room)
        {
            return new PublicRoom(room);
        }
        public static PublicBooking ToPublicBooking(this Booking booking)
        {
            return new PublicBooking(booking);
        }

        public static DateTime StartOfWeek(this DateTime dt)
        {
            int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime EndOfNextWeek(this DateTime dt)
        {
            var diff = (DayOfWeek.Friday - dt.DayOfWeek + 7);
            return dt.AddDays(diff);
        }

        public static User? GetUser(this ClaimsPrincipal princ)
        {
            string username = princ.FindFirstValue(ClaimTypes.NameIdentifier); // Note: Nicht Single line weil er sonst bs macht
            return Helpers.GetUser(username);
        }

        public static string GetUsername(this ClaimsPrincipal princ)
        {
            string username = princ.FindFirstValue(ClaimTypes.NameIdentifier); // Note: Nicht Single line weil er sonst bs macht
            return username;

        }
        public static bool IsInRole(this ClaimsPrincipal princ, string roleName)
        {
            return princ.FindAll(ClaimTypes.Role).Any(c => c.Type == ClaimTypes.Role && c.Value == roleName);
        }

        public static IEnumerable<Booking> GetBookings(this Room room)
        {
            try
            {
				using checkITContext ctx = new();
				return ctx.Bookings.Where(b => b.Room == room.Id).ToArray();
			}
            catch
            {
                return new List<Booking>().ToArray();
            }
        }

    }


}