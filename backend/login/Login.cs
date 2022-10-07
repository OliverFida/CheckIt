using awl_raumreservierung.core;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;

namespace awl_raumreservierung
{
	/// <summary>
	/// Loginklasse
	/// </summary>
	public static class Login
	{
		/// <summary>
		/// Prüft den Login eines Nutzers
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Gehashtes Passwort</param>
		/// <returns></returns>
		public static LoginMessage CheckLogin(string username, string password)
		{
			var ctx = Globals.DbContext;

			var user = ctx.Users.Where(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault();

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

		/// <summary>
		/// Mögliche Loginrückgabemeldungen
		/// </summary>
		public enum LoginMessage
		{
			/// Ungültige Anmeldedaten
			InvalidCredentials,

			/// Erfolgreich angemeldet als normaler User
			Success,

			/// User ist inaktiv
			InactiveUser,

			/// Erfolgreich angemeldet als Admin
			SuccessAsAdmin
		}
	}
}
