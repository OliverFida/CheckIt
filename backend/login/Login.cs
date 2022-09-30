using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;

namespace awl_raumreservierung
{
    public static class Login
    {
        public static LoginMessage CheckLogin(string username, string password)
        {
            checkITContext ctx = new();

            var user = ctx.Users
                            .Where(u => u.Username.ToLower() == username.ToLower())
                            .FirstOrDefault();

            if (user != null)
            {
                user.Lastlogon = DateTime.Now;
                ctx.SaveChanges();
            }

            // User mit username holen
            return user switch
            {
                { Active: false } => LoginMessage.InactiveUser,
                { Passwd: var pw, Role: var role } when pw == password => (role == UserRole.Admin) ? LoginMessage.SuccessAsAdmin : LoginMessage.Success,
                { Passwd: var pw } when pw == password => LoginMessage.Success,
                _ => LoginMessage.InvalidCredentials
            };
        }

        public enum LoginMessage
        {
            InvalidCredentials,
            Success,
            InactiveUser,
            SuccessAsAdmin
        }
    }
}
