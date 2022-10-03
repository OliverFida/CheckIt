using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Holt einen User aus der Datenbank
        /// </summary>
        /// <param name="id">User-ID</param>
        /// <returns></returns>
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

        /// <summary>
        /// Holt einen Raum aus der DB
        /// </summary>
        /// <param name="id">ID des Raums</param>
        /// <returns></returns>
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

        /// <summary>
        /// Holt ein Booking aus der DB
        /// </summary>
        /// <param name="id">Booking-ID</param>
        /// <returns></returns>
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

        /// <summary>
        /// Checkt, ob ein Booking sich mit einem anderen überschneidet
        /// </summary>
        /// <param name="model">Bookingmodel</param>
        /// <returns></returns>
        public static bool BookingOverlaps(CreateBookingModel model)
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

        /// <summary>
        /// Checkt, ob ein Booking sich mit einem anderen überschneidet
        /// </summary>
        /// <param name="booking">Booking</param>
        /// <returns></returns>
        public static bool BookingOverlaps(Booking booking)
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
