using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StatusCodeResult = Microsoft.AspNetCore.Mvc.StatusCodeResult;

namespace awl_raumreservierung.Controllers;

/// <summary>
///
/// </summary>
[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class bookingsController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	private readonly ILogger<bookingsController> _logger;

	private readonly checkITContext ctx;
	private readonly Helpers helper;

	/// <summary>
	///
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="_context"></param>
	public bookingsController(ILogger<bookingsController> logger, checkITContext _context)
	{
		_logger = logger;
				ctx = _context;
		helper = new Helpers(ctx);
	}

	/// <summary>
	/// Liefert ein Array von Buchungen für den angegebenen Raum in der Woche des angegebenen Tags.
	///
	/// </summary>
	/// <param name="roomId">Raum ID des Raums für den Buchungen ausgegeben werden.</param>
	/// <param name="model">Model der Daten</param>
	/// <returns> 'PublicBooking' Array für den Raum und Woche.</returns>
	[HttpPost("/rooms/{roomId}/bookings")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public PublicBooking[] Get(int roomId, GetBookingModel model)
	{
		try
		{
			if (!helper.DoesRoomExist(roomId))
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return Array.Empty<PublicBooking>();
			}

			if (model.EndDate is null)
			{
				model.EndDate = model.StartDate + new TimeSpan(6, 0, 0, 0);
			}
			Room room = helper.GetRoom(roomId);

			return room.GetBookings(ctx).Where(b => b.StartTime >= model.StartDate && b.EndTime <= model.EndDate).Select(b => b.ToPublicBooking(helper)).ToArray();
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return Array.Empty<PublicBooking>();
		}
	}

	/// <summary>
	/// Erstellt eine neue Buchung laut dem übergebeben Models für den angegebenen Benutzer.
	/// </summary>
	/// <param name="model">Model der Buchung die erstellt werden soll.</param>
	/// <returns>ReturnModel mit Statusnachricht und PublicBooking, wenn erfolgreich, in "Data".</returns>
	[HttpPut()]
	[Authorize]
	[SwaggerOperation(
		Description = "Ist der Benutzer 'null' wird die Buchung fuer den aktuellen Benutzer erstellt. "
			+ "Wenn nicht muss der aktuelle Benutzer ein Admin sein. Ist der Raum nicht existent oder aktiv, oder ueberlappt die "
			+ "Buchung sich mit einer bereits existierenden Buchung wird eine Fehlermeldung ausgegeben."
	)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ReturnModel Book(CreateBookingModel model)
	{
		try
		{
			if (!helper.DoesRoomExist(model.RoomID))
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel(new StatusCodeResult(404)) { Message = "Raum konnte nicht gefunden werden!" };
			}
			Room room = helper.GetRoom(model.RoomID);

			bool roomActive = room.Active;
			if (!roomActive)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return new ReturnModel(new StatusCodeResult(400)) { Message = "Der angegebene Raum ist aktuell nicht buchbar!" };
			}
			if (model.EndTime < model.StartTime)
			{
				return new ReturnModel(new StatusCodeResult(400)) { Message = "Endzeit darf nicht vor Startzeit sein!" };
			}
			bool overlapsWithOtherBookings = helper.BookingOverlaps(model, ctx);

			if (overlapsWithOtherBookings)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return new ReturnModel(new StatusCodeResult(400)) { Message = "Die angegebene Buchung überschneidet sich mit einer bereits bestehenden!" };
			}
			long userId;
			User currUser = User.GetUser(helper);
			if (!currUser.Active)
			{
				return new ReturnModel(new StatusCodeResult(404)) { Message = $"Benutzer ist inaktiv!" };
			}

			User user = helper.GetUser(model.Username);
			if (!helper.DoesUserExist(model.Username) || !helper.GetUser(model.Username).Active)
			{
				return new ReturnModel(new StatusCodeResult(404)) { Message = $"Benutzer {model.Username} konnte nicht gefunden werden oder ist inaktiv!" };
			}
			userId = user.Id;

			Booking book =
				new()
				{
					StartTime = model.StartTime,
					EndTime = model.EndTime,
					Room = model.RoomID,
					UserId = userId,
					CreateTime = DateTime.Now,
					CreatedBy = User.GetUser(helper)?.Id,
					Note = model.Note,
					StudentCount = model.StudentCount
				};
			ctx.Bookings.Add(book);

			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status201Created;
			return new ReturnModel(new StatusCodeResult(201)) { Message = $"Raum {room.Number} wurde erfolgreich für den {model.StartTime:d} gebucht.", Data = book.ToPublicBooking(helper) };
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
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
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ReturnModel Remove(int bookingId)
	{
		try
		{

			if (!helper.DoesBookingExist(bookingId))
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel(new StatusCodeResult(404)) { Message = "Buchung konnte nicht gefunden werden!" };
			}

			Booking booking = helper.GetBooking(bookingId);

			if (booking.UserId != User.GetUser(helper).Id && !User.IsInRole("Admin"))
			{
				Response.StatusCode = StatusCodes.Status401Unauthorized;
				return new ReturnModel(new StatusCodeResult(401)) { Message = "Es können nur eigene Buchungen gelöscht werden!" };
			}

			ctx.Bookings.Remove(booking);
			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel { Message = $"Buchung erfolgreich gelöscht!" };
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
		}
	}

	/// <summary>
	/// Bearbeitet eine existierenden Buchung.
	/// </summary>
	/// <param name="bookingId">ID der Buchung die bearbeitet werden soll.</param>
	/// <param name="model">Model mit den neuen Werten.</param>
	/// <returns>ReturnModel mit Statusnachricht und PublicBooking der bearbeiteten Buchung in "Data".</returns>
	[HttpPatch("{bookingId}")]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ReturnModel Edit(long bookingId, UpdateBookingModel model)
	{
		if (!helper.DoesBookingExist(bookingId))
		{
			Response.StatusCode = StatusCodes.Status404NotFound;
			return new ReturnModel(new StatusCodeResult(404)) { Message = "Buchung konnte nicht gefunden werden!" };
		}

		Booking booking = helper.GetBooking(bookingId);

		// user auth
		if (booking.UserId != User.GetUser(helper)?.Id && !User.IsInRole("Admin"))
		{
			return new ReturnModel(new StatusCodeResult(401)) { Message = "Keine Berechtigung!" };
		}
		// booking in future check
		if (model.EndTime < booking.StartTime)
		{
			return new ReturnModel(new StatusCodeResult(400)) { Message = "Neue Endzeit darf nicht vor Buchungsbeginn sein" };
		}
		bool inPast = booking.EndTime < DateTime.Now;
		if (inPast)
		{
			return new ReturnModel(new StatusCodeResult(400)) { Message = "Buchungszeitraum ist bereits abgelaufen." };
		}
		// no overlap check
		bool overlapsWithOtherBookings = helper.BookingOverlaps(booking, ctx);
		if (overlapsWithOtherBookings)
		{
			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel(new StatusCodeResult(400)) { Message = "Die angegebene Buchung Überschneidet sich mit einer bereits bestehenden!" };
		}
		booking.Note = model.Note;
		booking.EndTime = model.EndTime;
		booking.StudentCount = model.StudenCount;
		ctx.Bookings.Update(booking);
		ctx.SaveChanges();
		return new ReturnModel(new StatusCodeResult(201)) { Data = booking.ToPublicBooking(helper), Message = "Buchung erfolgreich bearbeitet!" };
	}
}
