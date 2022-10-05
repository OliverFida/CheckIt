using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.VisualBasic;

namespace awl_raumreservierung
{
	/// <summary>
	/// Stellt ReturnModels bereit
	/// </summary>
	public class ReturnModel
	{
		/// <summary>
		/// Erstellt eine neues Returnmodel mit Statuscode 200
		/// </summary>
		/// <returns></returns>
		public ReturnModel() : this(new StatusCodeResult(200)) { }

		/// <summary>
		/// Erstellt eine neues Returnmodel mit bestimmten Statuscude
		/// </summary>
		/// <param name="statusCode">Statuscode</param>
		public ReturnModel(StatusCodeResult statusCode)
		{
			Status = statusCode.StatusCode;
			if (Status is < 300 and > 199)
			{
				StatusMessage = "success";
			}
			else
			{
				StatusMessage = ReasonPhrases.GetReasonPhrase(Status);
			}

			Message = string.Empty;
		}

		/// <summary>
		/// Statuscode
		/// </summary>
		/// <value></value>
		public int Status { get; set; }

		/// <summary>
		/// HTTP-Statusmessage
		/// </summary>
		/// <value></value>
		public string StatusMessage { get; set; }

		/// <summary>
		/// Lesbare Nachricht
		/// </summary>
		/// <value></value>
		public string Message { get; set; }

		/// <summary>
		/// Zusätzliche Daten
		/// </summary>
		/// <value></value>
		public object? Data { get; set; }
	}

	/// <summary>
	/// Useranlagemodel
	/// </summary>
	public class CreateUserModel
	{
		/// <summary>
		/// Username
		/// </summary>
		/// <value></value>
		public string? Username { get; set; }

		/// <summary>
		/// Vorname
		/// </summary>
		/// <value></value>
		public string? FirstName { get; set; }

		/// <summary>
		/// Nachname
		/// </summary>
		/// <value></value>
		public string? LastName { get; set; }

		/// <summary>
		/// Rolle
		/// </summary>
		/// <value></value>
		public UserRole Role { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		/// <value></value>
		public string? Password { get; set; }
	}

	/// <summary>
	/// Loginmodel
	/// </summary>
	public class LoginUserModel
	{
		/// <summary>
		/// Username
		/// </summary>
		/// <value></value>
		public string Username { get; set; }

		/// <summary>
		/// Passwort
		/// </summary>
		/// <value></value>
		public string Password { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public LoginUserModel(string username, string password)
		{
			Username = username;
			Password = password;
		}
	}

	/// <summary>
	/// Userupdatemodel
	/// </summary>
	public class UpdateUserModel
	{
		/// <summary>
		/// Vorname
		/// </summary>
		/// <value></value>
		public string? FirstName { get; set; }

		/// <summary>
		/// Nachname
		/// </summary>
		/// <value></value>
		public string? LastName { get; set; }

		/// <summary>
		/// Rolle
		/// </summary>
		/// <value></value>
		public UserRole Role { get; set; }
	}

	/// <summary>
	/// Buchungsmodel
	/// </summary>
	public class CreateBookingModel
	{
		private DateTime startTime;
		private DateTime endTime;

		/// <summary>
		/// Raum-ID
		/// </summary>
		/// <value></value>
		public int RoomID { get; set; }

		/// <summary>
		/// Startzeit
		/// </summary>
		/// <value></value>
		public DateTime StartTime { get => startTime; set => startTime = DateTime.SpecifyKind(value, DateTimeKind.Utc); }

		/// <summary>
		/// Endzeit
		/// </summary>
		/// <value></value>
		public DateTime EndTime { get => endTime; set => endTime = DateTime.SpecifyKind(value, DateTimeKind.Utc); }

		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string? Note { get; set; }
		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string? Username { get; set; }
	}
	/// <summary>
	/// Update Buchungsmodel
	/// </summary>
	public class UpdateBookingModel
	{
		private DateTime endTime;

		/// <summary>
		/// Endzeit
		/// </summary>
		/// <value></value>
		public DateTime EndTime { get => endTime; set => endTime = DateTime.SpecifyKind(value, DateTimeKind.Utc); }

		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string? Note { get; set; }
	}
	/// <summary>
	/// Raummodel
	/// </summary>
	public class CreateRoomModel
	{
		/// <summary>
		/// Raumnummer
		/// </summary>
		/// <value></value>
		public string? Number { get; set; }

		/// <summary>
		/// /// Raumname
		/// </summary>
		/// <value></value>
		public string? Name { get; set; }

		/// <summary>
		/// Aktivstatus
		/// </summary>
		/// <value></value>
		public bool Active { get; set; }
	}

	/// <summary>
	/// Model zum Holen von Bookings
	/// </summary>
	public class GetBookingModel
	{
		/// <summary>
		/// Startdatum der Abfrage, default Montag der aktuellen Woche
		/// </summary>
		/// <value></value>
		public DateTime StartDate { get; set; } = DateTime.Now.StartOfWeek();

		/// <summary>
		/// Enddatum der Abfrage, default das Ende der Woche von Startdate
		/// </summary>
		/// <returns></returns>
		public DateTime? EndDate { get; set; }
	}
}
