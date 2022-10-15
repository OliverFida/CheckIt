
namespace awl_raumreservierung.classes {
	/// <summary>
	/// Loginklasse
	/// </summary>
	public static class Login {
		/// <summary>
		/// Prüft den Login eines Nutzers
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Gehashtes Passwort</param>
		/// <param name="ctx">DB-Context</param>
		/// <returns></returns>
		public static LoginMessage CheckLogin(string username, string password, checkITContext ctx) {
			var user = ctx.Users.Where(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault();

			if(user != null) {
				user.Lastlogon = DateTime.Now;
				ctx.SaveChanges();
			}

			// User mit username holen
			return user switch { 
				{ Active: false } => LoginMessage.InactiveUser,
				{ Role: UserRole.Admin } when user.IsPasswordCorrect(password) => LoginMessage.SuccessAsAdmin, 
				{ Role: UserRole.User } when user.IsPasswordCorrect(password)  => LoginMessage.Success,
				_ => LoginMessage.InvalidCredentials
			};
		}

		/// <summary>
		/// Mögliche Loginrückgabemeldungen
		/// </summary>
		public enum LoginMessage {
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
