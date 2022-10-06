using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
	/// <summary>
	/// DB-Booking
	/// </summary>
	public partial class Booking
	{
		private DateTime endTime;

		/// <summary>
		/// Booking-ID
		/// </summary>
		/// <value></value>
		public long Id { get; set; }

		/// <summary>
		/// Startzeit
		/// </summary>
		/// <value></value>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Endzeit, eine Sekunde wird abgezogen.
		/// </summary>
		/// <value></value>
		public DateTime EndTime { get => endTime; set => endTime = value.Subtract(new TimeSpan(0,0,1)); }

		/// <summary>
		/// Raum-ID
		/// </summary>
		/// <value></value>
		public long Room { get; set; }

		/// <summary>
		/// ID des Bucher
		/// </summary>
		/// <value></value>
		public long UserId { get; set; }

		/// <summary>
		/// Erstellungsdatum
		/// </summary>
		/// <value></value>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// Ersteller
		/// </summary>
		/// <value></value>
		public long? CreatedBy { get; set; }

		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string? Note { get; set; }

		/// <summary>
		///
		/// </summary>
		public Booking() { }

		/// <summary>
		///
		/// </summary>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <param name="room"></param>
		/// <param name="userId"></param>
		/// <param name="createTime"></param>
		/// <param name="createdBy"></param>
		/// <param name="note"></param>
		public Booking(DateTime startTime, DateTime endTime, long room, long userId, DateTime createTime, long? createdBy, string note)
		{
			StartTime = startTime;
			EndTime = endTime;
			Room = room;
			UserId = userId;
			CreateTime = createTime;
			CreatedBy = createdBy;
			Note = note;
		}
	}
}
