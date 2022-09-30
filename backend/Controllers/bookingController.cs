using awl_raumreservierung.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using static awl_raumreservierung.Controllers.adminController;

namespace awl_raumreservierung.Controllers;


[ApiController]
[Route("[controller]")]
public class bookingController : ControllerBase
{
	private readonly ILogger<bookingController> _logger;
	private readonly checkITContext ctx;

	public bookingController(ILogger<bookingController> logger)
	{
		ctx = new checkITContext();
		_logger = logger;
	}

	[HttpGet("room/{roomId}")]
	[Authorize]
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
			return new PublicBooking[0];
		}
	}

	[HttpPut("book")]
	[Authorize]
	public ReturnModel book(CreateBookingModel model)
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


			// TODO : Check if room is active
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

			Booking book = new()
			{
				StartTime = model.StartTime,
				EndTime = model.EndTime,
				Room = model.RoomID,
				UserId = User.GetUser()?.Id ?? 0L,
				CreateTime = DateTime.Now,
				CreatedBy = User.GetUser()?.Id
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

	[HttpDelete("{bookingId}")]
	[Authorize]
	public ReturnModel remove(int bookingId)
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
	[HttpPut("bookAsAdmin")]
	[Authorize(Roles = "Admin")]
	public ReturnModel bookAsAdmin(CreateBookingModel model)
	{
		bool overlapsWithOtherBookings = Helpers.BookingOverlaps(model);
		if (overlapsWithOtherBookings)
		{
			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel(new StatusCodeResult(400))
			{
				Message = "Die angegebene Buchung überschneidet sich mit einer bereits bestehenden!"
			};
		}
		Booking booking = new()
		{
			StartTime = model.StartTime,
			EndTime = model.EndTime,
			Room = model.RoomID,
			UserId = User.GetUser()?.Id ?? 0L,
			CreateTime = DateTime.Now,
			CreatedBy = User.GetUser()?.Id
		};

		ctx.Bookings.Add(booking);
		ctx.SaveChanges();
		return new ReturnModel()
		{
			Message = "Buchung erfolgreich"
		};
	}
	[HttpPost("edit")]
	public ReturnModel edit(long bookingId, DateTime EndTime)
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
		bool inPast = (booking != null) && (booking.EndTime > DateTime.Now);
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
		booking.EndTime = EndTime;
		ctx.SaveChanges();
		return new ReturnModel(new StatusCodeResult(201))
		{ 
			Message = "Buchung erfolgreich bearbeitet!"
		};
	}
}