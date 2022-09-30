using awl_raumreservierung.classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static awl_raumreservierung.Controllers.adminController;

namespace awl_raumreservierung.Controllers;


[ApiController]
[Route("[controller]")]
public class bookingController : ControllerBase
{
	private readonly ILogger<bookingController> _logger;

	public bookingController(ILogger<bookingController> logger)
	{
		_logger = logger;
	}

	[HttpGet("roomBookings")]
	[Authorize]
	public Booking[] Get(int roomId)
	{
		var db = new checkITContext();
		var res = db.Bookings.AsEnumerable().Where(s => s.Room == roomId);
		var after = res.Where(s => s.StartTime >= bookingHelper.StartOfWeek(DateTime.Now));
		var before = after.Where(s=> s.EndTime <= bookingHelper.endNextWeek(DateTime.Now));
		return before.ToArray();
	}
	[HttpPut("book")]
	[Authorize]
	public ReturnModel book(int roomId, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
	{
		var db = new checkITContext();
		if (db.Rooms.Where(r => r.Id == roomId).FirstOrDefault().active ==1) { 
		var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var userId = userHelper.getUserId(authUsername);
		if (userId < 0)
		{
			return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
			{
				message = $"Benutzer {authUsername} nicht gefunden."
			};
		}
		var booking = new Booking(startTime, endTime, roomId, userId, createTime, createdBy);
		db.Bookings.Add(booking);
		db.SaveChanges();
		return new ReturnModel(StatusCode(StatusCodes.Status201Created))
		{
			message = "Buchung erfolgreich."
		};
		}else
		{
		return new ReturnModel()
		{
			message = "Raum ist nicht Buchbar."
		};
		}
	}

	[HttpDelete("remove")]
	[Authorize]
	public ReturnModel remove(int bookingId)
	{
		var db = new checkITContext();
		var booking = db.Bookings.Where(b => b.Id == bookingId).FirstOrDefault();
		if (booking != null)
		{

			var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userId = userHelper.getUserId(authUsername);
			if (userId < 0)
			{
				return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
				{
					message = $"Benutzer {authUsername} nicht gefunden."
				};
			}
			if (userId == booking.UserId || userHelper.isAdmin())
			{
				if (booking != null)
				{
					db.Bookings.Remove(booking);
					db.SaveChanges();
				}
				return new ReturnModel(StatusCode(StatusCodes.Status200OK))
				{
					message = "Buchung erfolgreich entfernt."
				};
			}
			else
			{
				return new ReturnModel(StatusCode(StatusCodes.Status401Unauthorized))
				{
					message = "Keine Berechtigung."
				};
			}
		}
		else
		{
			return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
			{
				message = "Buchung nicht gefunden."
			};
		}
	}
	[HttpPut("bookAsAdmin")]
	[Authorize(Roles = "Adminstrator")]
	public ReturnModel bookAsAdmin(int roomId, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
	{
		var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var userId = userHelper.getUserId(authUsername);
		if (userId < 0)
		{
			return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
			{ 
				message = $"Benutzer {authUsername} nicht gefunden."
			};
		}
		var db = new checkITContext();
		var booking = new Booking(startTime, endTime, roomId, userId, createTime, createdBy);
		db.Bookings.Add(booking);
		db.SaveChanges();
		return new ReturnModel(StatusCode(StatusCodes.Status200OK))
		{
			message = "Buchung erfolgreich."
		};
	}
	[HttpPost("edit")]
	public ReturnModel edit(DateTime startTime,DateTime newEndTime)
	{
		var db = new checkITContext();
		var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var booking = db.Bookings.Where(b => b.StartTime == startTime).FirstOrDefault();
		var userId = userHelper.getUserId(authUsername);
		if (userId < 0)
		{
			return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
			{
				message = $"Benutzer {authUsername} nicht gefunden."
			};
		}
		if (booking == null)
		{
			return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
			{
				message = "Keine Buchung zum angegebenen Zeitpunkt."
			};
		}
		// user auth
		if (userId == booking.UserId || userHelper.isAdmin())
		{
			// booking in future check
			if (booking != null && booking.EndTime > DateTime.Now)
			{
				// no overlap check
				if (db.Bookings.Where(b => b.StartTime > startTime && b.StartTime < newEndTime).Any())
				{
					booking.EndTime = newEndTime;
					db.SaveChanges();
					return new ReturnModel(StatusCode(StatusCodes.Status200OK))
					{
						message = "Buchung erfolgreich bearbeitet."
					};
				}
				else
				{
					return new ReturnModel(StatusCode(StatusCodes.Status409Conflict))
					{
						message = "Neue Endzeit kolidiert mit einer anderen Buchung."
					};
				}
			}
			else
			{
				return new ReturnModel
				{
					status = 400,
					statusMessage = "error",
					message = "Buchung abgelaufen."
				};
			}
		}
		else
		{
			return new ReturnModel
			{
				message = "Keine Berechtigung."
			};
		}
	}
}