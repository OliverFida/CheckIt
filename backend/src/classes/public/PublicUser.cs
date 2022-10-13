namespace awl_raumreservierung
{
	/// <summary>
	/// Bildet die öffentlichen Daten eines Nutzers ab
	/// </summary>
	public class PublicUser
	{
		private DateTime? lastLogon;
		private DateTime? lastchange;

		/// <summary>
		/// Username
		/// </summary>
		/// <value></value>
		public string Username { get; set; }

		/// <summary>
		/// Vorname
		/// </summary>
		/// <value></value>
		public string FirstName { get; set; }

		/// <summary>
		/// Nachname
		/// </summary>
		/// <value></value>
		public string Lastname { get; set; }

		/// <summary>
		/// Letzte Loginzeit in Localtime
		/// </summary>
		/// <value></value>
		public DateTime? LastLogon { get => DateTime.SpecifyKind(lastLogon ?? DateTime.MinValue, DateTimeKind.Local).ToUniversalTime(); set => lastLogon = value; }

		/// <summary>
		/// Letzte Änderung der Nutzerdaten in Localtime
		/// </summary>
		/// <value></value>
		public DateTime? Lastchange { get => DateTime.SpecifyKind(lastchange ?? DateTime.MinValue, DateTimeKind.Utc).ToUniversalTime(); set => lastchange = value; }

		/// <summary>
		/// Rolle des Users
		/// </summary>
		/// <value></value>
		public UserRole Role { get; set; }

		/// <summary>
		/// Wahr, falsch der User aktivgesetzt ist
		/// </summary>
		/// <value></value>
		public bool Active { get; set; }

		/// <summary>
		///
		/// /// </summary>
		/// <param name="user"></param>
		public PublicUser(User user)
		{
			Username = user.Username;
			FirstName = user.Firstname;
			Lastname = user.Lastname;
			LastLogon = user.Lastlogon;
			Lastchange = user.Lastchange;
			Role = user.Role;
			Active = user.Active;

		}
	}
}
