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
	public StatusCodeResult book(int roomId, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
	{
		var db = new checkITContext();
		// if (db.Rooms.Where(r => r.Id == roomId).FirstOrDefault().active)	TODO: DB eintrag "active"
		var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var userId = userHelper.getUserId(authUsername);
		if (userId < 0)
		{
			return StatusCode(StatusCodes.Status404NotFound);
		}
		var booking = new Booking(startTime, endTime, roomId, userId, createTime, createdBy);

		db.Bookings.Add(booking);
		db.SaveChanges();
		return StatusCode(StatusCodes.Status201Created);
	}

	[HttpDelete("remove")]
	[Authorize]
	public StatusCodeResult remove(int bookingId)
	{
		var db = new checkITContext();
		var booking = db.Bookings.Where(b => b.Id == bookingId).FirstOrDefault();
		if (booking != null)
		{
			db.Bookings.Remove(booking);
			db.SaveChanges();
		}
		return StatusCode(StatusCodes.Status200OK);
	}
	[HttpPut("bookAsAdmin")]
	[Authorize(Roles = "Adminstrator")]
	public StatusCodeResult bookAsAdmin(int roomId, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
	{
		var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var userId = userHelper.getUserId(authUsername);
		if (userId < 0)
		{
			return StatusCode(StatusCodes.Status404NotFound);
		}
		var db = new checkITContext();
		var booking = new Booking(startTime, endTime, roomId, userId, createTime, createdBy);
		db.Bookings.Add(booking);
		db.SaveChanges();
		return StatusCode(StatusCodes.Status201Created);
	}
	[HttpPost("edit")]
	public StatusCodeResult edit( DateTime startTime,DateTime newEndTime)
	{
		var db = new checkITContext();
		var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var booking = db.Bookings.Where(b => b.StartTime == startTime).FirstOrDefault();
		var userId = userHelper.getUserId(authUsername);
		if (userId < 0)
		{
			return StatusCode(StatusCodes.Status404NotFound);
		}
		if (booking == null)
		{
			return StatusCode(StatusCodes.Status404NotFound);
		}
		// user auth
		var isAdmin = User.FindAll(ClaimTypes.Role).Any(c => c is { Type: ClaimTypes.Role } and { Value: "Admin" });

		if (userId == booking.UserId || isAdmin)
		{
			// booking in future check
			if (booking != null && booking.EndTime > DateTime.Now)
			{
				// no overlap check
				if (db.Bookings.Where(b => b.StartTime > startTime && b.StartTime < startTime).Any())
				{
					booking.EndTime = newEndTime;
					db.SaveChanges();
				}
			}
			else
			{
			return StatusCode(StatusCodes.Status404NotFound);
			}
		return StatusCode(StatusCodes.Status200OK);
		}
		return StatusCode(StatusCodes.Status200OK);
	}
}