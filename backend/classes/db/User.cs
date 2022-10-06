using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
	/// <summary>
	///
	/// </summary>
	public partial class User
	{
		/// <summary>
		/// User-ID
		/// </summary>
		/// <value></value>
		public long Id { get; set; }

		/// <summary>
		/// Username
		/// </summary>
		/// <value></value>
		public string Username { get; set; } = null!;

		/// <summary>
		/// Vorname
		/// </summary>
		/// <value></value>
		public string Firstname { get; set; } = null!;

		/// <summary>
		/// Nachname
		/// </summary>
		/// <value></value>
		public string Lastname { get; set; } = null!;

		/// <summary>
		/// Passwort
		/// </summary>
		/// <value></value>
		public string Passwd { get; set; } = null!;

		/// <summary>
		/// Letzer Logon
		/// </summary>
		/// <value></value>
		public DateTime? Lastlogon { get; set; }

		/// <summary>
		/// Letzte Datenänderung
		/// </summary>
		/// <value></value>
		public DateTime? Lastchange { get; set; }

		/// <summary>
		/// Ist User aktiv
		/// </summary>
		/// <value></value>
		public bool Active { get; set; }

		/// <summary>
		/// Userrolle
		/// </summary>
		/// <value></value>
		public UserRole Role { get; set; }
	}

	/// <summary>
	/// Mögliche Userrollen
	/// </summary>
	public enum UserRole
	{
		/// Standardbenutzer
		User,

		/// Admin
		Admin
	}
}
