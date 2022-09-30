using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace awl_raumreservierung
{
    public class Helpers
    {
        public static User? GetUser(string? username)
        {
            try
            {
                if (username is null)
                {
                    throw new Exception("Username nicht angegeben.");
                }
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
                checkITContext ctx = new();
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
                checkITContext ctx = new();
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
                checkITContext ctx = new();
                {
                    return ctx.Bookings.Where(b => b.Id == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
