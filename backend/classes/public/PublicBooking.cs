namespace awl_raumreservierung
{
	/// <summary>
	/// Öffentliche Daten einer Buchung
	/// </summary>
	public class PublicBooking
	{
		/// <summary>
		/// ID
		/// </summary>
		/// <value></value>
		public long Id { get; set; }

		/// <summary>
		/// Startzeit in Serverzeit
		/// </summary>
		/// <value></value>
		public DateTime? StartTime { get; set; }

		/// <summary>
		/// Endzeit in Serverzeit
		/// </summary>
		/// <value></value>
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// Raum
		/// </summary>
		/// <value></value>
		public PublicRoom Room { get; set; }

		/// <summary>
		/// Bucher
		/// </summary>
		/// <value></value>
		public PublicUser User { get; set; }

		/// <summary>
		/// Erstellungsdatum in Serverzeit
		/// </summary>
		/// <value></value>
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string? Note { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="booking"></param>
		public PublicBooking(Booking booking)
		{
			this.Id = booking.Id;
			this.StartTime = booking.StartTime;
			this.EndTime = booking.EndTime;
			this.Room = Helpers.GetRoom(booking.Room).ToPublicRoom();

			var user = Helpers.GetUser(booking.UserId.ToInt()).ToPublicUser();

			if (user.Username is null)
			{
				user = new PublicUser(new User { })
				{
					FirstName = "Gelöschter",
					Lastname = "Benutzer",
					Username = "deleted"
				};
			}

			this.User = user;
			this.CreateTime = booking.CreateTime;
			this.Note = booking.Note;
		}
	}
}
