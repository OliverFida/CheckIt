using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Web.Http.Results;
using static awl_raumreservierung.Controllers.adminController;
using StatusCodeResult = Microsoft.AspNetCore.Mvc.StatusCodeResult;

namespace awl_raumreservierung.Controllers;


[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class bookingsController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	private readonly ILogger<bookingsController> _logger;
	private readonly checkITContext ctx;

	public bookingsController(ILogger<bookingsController> logger)
	{
		ctx = new checkITContext();
		_logger = logger;
	}
	/// <summary>
	/// Liefert ein Array von Buchungen fuer den angegebenen Raum in der Woche des angegebenen Tags.
	/// 
	/// </summary>
	/// <param name="roomId">Raum ID des Raums fuer den Buchungen ausgegeben werden.</param>
	/// <param name="date">Ein Tag der Woche fuer die Buchungen ausgegeben werden.</param>
	/// <returns> 'PublicBooking' Array fuer den Raum und Woche.</returns>
	[HttpGet("/rooms/{roomId}/bookings")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public PublicBooking[] Get(int roomId, DateTime? date)
	{
		try
		{
			Room? room = Helpers.GetRoom(roomId);
			if (room == null)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return Array.Empty<PublicBooking>();
			}

			if (date == null)
			{
				date = DateTime.Now.StartOfWeek();
			}
			date = date.Value.StartOfWeek();

			return room.GetBookings()
						  .Where(b =>
								 b.StartTime >= date.Value &&
								 b.EndTime <= date.Value.AddDays(6)
							).Select(b => b.ToPublicBooking()).ToArray();
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return Array.Empty<PublicBooking>();
		}
	}
	/// <summary>
	/// Erstellt eine neue Buchung laut dem uebergebeben Models fuer den angegebenen Benutzer.
	/// </summary>
	/// <param name="model">Model der Buchung die erstellt werden soll.</param>
	/// <param name="username"></param>
	/// <returns>ReturnModel mit Statusnachricht und PublicBooking, wenn erfolgreich, in "Data".</returns>
	[HttpPut()]
	[Authorize]
	[SwaggerOperation(Description ="Ist der Benutzer 'null' wird die Buchung fuer den aktuellen Benutzer erstellt. " +
		"Wenn nicht muss der aktuelle Benutzer ein Admin sein. Ist der Raum nicht existent oder aktiv, oder ueberlappt die " +
		"Buchung sich mit einer bereits existierenden Buchung wird eine Fehlermeldung ausgegeben.")]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ReturnModel Book(CreateBookingModel model)
	{
		try
		{
			Room? room = Helpers.GetRoom(model.RoomID);
			if (room == null)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel(new StatusCodeResult(404))
				{
					Message = "Raum konnte nicht gefunden werden!"
				};
			}

			bool roomActive = room.Active;
			if (!roomActive)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return new ReturnModel(new StatusCodeResult(400))
				{
					Message = "Der angegebene Raum ist aktuell nicht buchbar!"
				};

			}

			bool overlapsWithOtherBookings = Helpers.BookingOverlaps(model);

			if (overlapsWithOtherBookings)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return new ReturnModel(new StatusCodeResult(400))
				{
					Message = "Die angegebene Buchung überschneidet sich mit einer bereits bestehenden!"
				};
			}
			long userId;
			User? currUser = User.GetUser();
			if (currUser is null || !currUser.Active)
			{
				return new ReturnModel(new StatusCodeResult(404))
				{
					Message = $"Benutzer konnte nicht gefunden werden oder ist inaktiv!"
				};
			}

			if (model.Username is null)
			{
				userId = User.GetUser()?.Id ?? 0L;
			}
			else
			{
				User? user =Helpers.GetUser(model.Username);
				if (user is null || !user.Active)
				{
					return new ReturnModel(new StatusCodeResult(404))
					{
						Message = $"Benutzer {model.Username} konnte nicht gefunden werden oder ist inaktiv!"
					};
				}
				userId = user.Id;
			}

			Booking book = new()
			{
				StartTime = model.StartTime,
				EndTime = model.EndTime,
				Room = model.RoomID,
				UserId = userId,
				CreateTime = DateTime.Now,
				CreatedBy = User.GetUser()?.Id,
				Note = model.Note
			};
			ctx.Bookings.Add(book);

			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status201Created;
			return new ReturnModel(new StatusCodeResult(201))
			{
				Message = $"Raum {room.Number} wurde erfolgreich für den {model.StartTime:d} gebucht.",
				Data = book.ToPublicBooking()
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel(new StatusCodeResult(400))
			{
				StatusMessage = "error",
				Message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}
	/// <summary>
	/// Entfernt eine Buchung aus der Datebank.
	/// </summary>
	/// <param name="bookingId">ID der Buchung die entfernt werden soll.</param>
	/// <returns>ReturnModel mit Statusnachricht.</returns>
	[HttpDelete("{bookingId}")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ReturnModel Remove(int bookingId)
	{
		try
		{
			Booking? booking = Helpers.GetBooking(bookingId);

			if (booking is null)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel(new StatusCodeResult(404))
				{
					Message = "Buchung konnte nicht gefunden werden!"
				};
			}

			if (booking.UserId != User.GetUser()?.Id && !User.IsInRole("Admin"))
			{
				Response.StatusCode = StatusCodes.Status401Unauthorized;
				return new ReturnModel(new StatusCodeResult(401))
				{
					Message = "Es können nur eigene Buchungen gelöscht werden!"
				};
			}

			ctx.Bookings.Remove(booking);
			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel
			{
				Message = $"Buchung erfolgreich gelöscht!"
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel()
			{
				Status = 400,
				StatusMessage = "error",
				Message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}
	/// <summary>
	/// Bearbeitet eine existierenden Buchung.
	/// </summary>
	/// <param name="bookingId">ID der Buchung die bearbeitet werden soll.</param>
	/// <param name="EndTime">Neue Endzeit der Buchung.</param>
	/// <param name="note">Notiz welche die existierende Notiz ersetzt.</param>
	/// <returns>ReturnModel mit Statusnachricht und PublicBooking der bearbeiteten Buchung in "Data".</returns>
	[HttpPatch("{bookingId}")]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ReturnModel Edit(long bookingId, DateTime EndTime, string? note)
	{
		Booking? booking = Helpers.GetBooking(bookingId);
		if (booking is null)
		{
			Response.StatusCode = StatusCodes.Status404NotFound;
			return new ReturnModel(new StatusCodeResult(404))
			{
				Message = "Buchung konnte nicht gefunden werden!"
			};
		}
		// user auth
		if (booking.UserId != User.GetUser()?.Id && !User.IsInRole("Admin"))
		{
			return new ReturnModel(new StatusCodeResult(401))
			{
				Message = "Keine Berechtigung!"
			};
		}
		// booking in future check
		bool inPast = booking.EndTime > DateTime.Now;
		if (inPast)
		{
			return new ReturnModel(new StatusCodeResult(400))
			{
				Message = "Buchungszeitraum ist bereits abgelaufen."
			};
		}
		// no overlap check
		bool overlapsWithOtherBookings = Helpers.BookingOverlaps(booking);
		if (overlapsWithOtherBookings)
		{
			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel(new StatusCodeResult(400))
			{
				Message = "Die angegebene Buchung überschneidet sich mit einer bereits bestehenden!"
			};
		}
		booking.Note = note;
		booking.EndTime = EndTime;
		ctx.SaveChanges();
		return new ReturnModel(new StatusCodeResult(201))
		{
			Data = booking.ToPublicBooking(),
			Message = "Buchung erfolgreich bearbeitet!"
		};
	}
}