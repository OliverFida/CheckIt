using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;

namespace awl_raumreservierung {
	/// <summary>
	/// Stellt ReturnModels bereit
	/// </summary>
	public class ReturnModel {
		/// <summary>
		/// Erstellt eine neues Returnmodel mit Statuscode 200
		/// </summary>
		/// <returns></returns>
		public ReturnModel() : this(new StatusCodeResult(200)) { }

		/// <summary>
		/// Erstellt eine neues Returnmodel mit bestimmten Statuscude
		/// </summary>
		/// <param name="statusCode">Statuscode</param>
		public ReturnModel(StatusCodeResult statusCode) {
			Status = statusCode.StatusCode;
			StatusMessage = Status is < 300 and > 199 ? "success" : ReasonPhrases.GetReasonPhrase(Status);

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
	public class CreateUserModel {
		/// <summary>
		/// Username
		/// </summary>
		/// <value></value>
		public string Username { get; set; } = null!;

		/// <summary>
		/// Vorname
		/// </summary>
		/// <value></value>
		public string FirstName { get; set; } = null!;

		/// <summary>
		/// Nachname
		/// </summary>
		/// <value></value>
		public string LastName { get; set; } = null!;

		/// <summary>
		/// Rolle
		/// </summary>
		/// <value></value>
		public UserRole Role { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		/// <value></value>
		public string Password { get; set; } = null!;

	}

	/// <summary>
	/// Loginmodel
	/// </summary>
	public class LoginUserModel {
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
		public LoginUserModel(string username, string password) {
			Username = username;
			Password = password;
		}
	}

	/// <summary>
	/// Userupdatemodel
	/// </summary>
	public class UpdateUserModel {
		/// <summary>
		/// Vorname
		/// </summary>
		/// <value></value>
		public string FirstName { get; set; } = null!;

		/// <summary>
		/// Nachname
		/// </summary>
		/// <value></value>
		public string LastName { get; set; } = null!;

		/// <summary>
		/// Rolle
		/// </summary>
		/// <value></value>
		public UserRole Role { get; set; }
	}

	/// <summary>
	/// Buchungsmodel
	/// </summary>
	public class CreateBookingModel {
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
		public DateTime StartTime {
			get => startTime;
			set => startTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		/// <summary>
		/// Endzeit
		/// </summary>
		/// <value></value>
		public DateTime EndTime {
			get => endTime;
			set => endTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string? Note { get; set; }

		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string Username { get; set; }

		/// <summary>
		/// Schüleranzahl
		/// </summary>
		/// <value></value>
		public int? StudentCount { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		public CreateBookingModel(string username) => Username = username;
	}

	/// <summary>
	/// Update Buchungsmodel
	/// </summary>
	public class UpdateBookingModel {
		private DateTime endTime;

		/// <summary>
		/// Endzeit
		/// </summary>
		/// <value></value>
		public DateTime EndTime {
			get => endTime;
			set => endTime = DateTime.SpecifyKind(value, DateTimeKind.Utc);
		}

		/// <summary>
		/// Notiz
		/// </summary>
		/// <value></value>
		public string? Note { get; set; }

		/// <summary>
		/// Schüleranzahl
		/// </summary>
		/// <value></value>
		public int StudentCount { get; set; }
	}

	/// <summary>
	/// Raummodel
	/// </summary>
	public class CreateRoomModel {
		/// <summary>
		/// Raumnummer
		/// </summary>
		/// <value></value>
		public string Number { get; set; }

		/// <summary>
		/// /// Raumname
		/// </summary>
		/// <value></value>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="number"></param>
		public CreateRoomModel(string name, string number) {
			Name = name;
			Number = number;
		}

		/// <summary>
		/// Aktivstatus
		/// </summary>
		/// <value></value>
		public bool Active { get; set; }
	}

	/// <summary>
	/// Model zum Holen von Bookings
	/// </summary>
	public class GetBookingModel {
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

	/// <summary>
	/// Model für Passwortübergabe
	/// </summary>
	public class PasswordModel {
		/// <summary>
		/// Passworthash
		/// </summary>
		/// <value></value>
		[Required]
		public string Password { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="password"></param>
		public PasswordModel(string password) => Password = password;
	}
}
