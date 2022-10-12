﻿using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
	/// <summary>
	///
	/// </summary>
	public partial class User
	{
		private string username = null!;

		/// <summary>
		/// User-ID
		/// </summary>
		/// <value></value>
		public long Id { get; set; }

		/// <summary>
		/// Username
		/// </summary>
		/// <value></value>
		public string Username { get => username.ToLower(); set => username = value.ToLower(); }
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

		/// <summary>
		/// 
		/// </summary>
		public User()
		{
			DateTime.SpecifyKind(Lastlogon ?? DateTime.Now, DateTimeKind.Utc);
			DateTime.SpecifyKind(Lastchange ?? DateTime.Now, DateTimeKind.Utc);
		}
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
