using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace awl_raumreservierung
{
    public class Helpers
    {
        public static User? GetUser(string username)
        {
            try
            {
				using checkITContext ctx = new();
				return ctx.Users
					 .Where(u => u.Username.ToLower().Trim() == username.ToLower().Trim())
					 .FirstOrDefault();
			}
            catch
            {
                return null;
            }
        }

        public static User? GetUser(int id)
        {
            try
            {
                using checkITContext ctx = new();
                return ctx.Users.Where(u => u.Id == id).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static Room? GetRoom(long id)
        {
            try
            {
                using checkITContext ctx = new();
                return ctx.Rooms.Where(r => r.Id == id).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static Booking? GetBooking(long id)
        {
            try
            {
                using checkITContext ctx = new();
                return ctx.Bookings.Where(b => b.Id == id).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static Boolean BookingOverlaps(CreateBookingModel model)
        {
            var room = GetRoom(model.RoomID);
            if (room is null)
            {
                return false;
            }

            bool overlapsWithOtherBookings = room.GetBookings()
                .Any(b => b.StartTime <= model.EndTime && model.StartTime <= b.EndTime);
            return overlapsWithOtherBookings;
        }

        public static Boolean BookingOverlaps(Booking booking)
        {
            var room = GetRoom(booking.Id);
            if (room is null)
            {
                return false;
            }

            bool overlapsWithOtherBookings = room.GetBookings()
                .Any(b => b.StartTime <= booking.EndTime && booking.StartTime <= b.EndTime);
            return overlapsWithOtherBookings;
        }
    }
}
