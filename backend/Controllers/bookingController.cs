using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
		return db.Bookings.Where(s => s.Room == roomId).ToArray();
	}
	[HttpPut("book")]
	[Authorize]
	public StatusCodeResult book(int roomId, string username, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
	{
		var userId = getUserId(username);
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

	[HttpPut("remove")]
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
	public StatusCodeResult bookAsAdmin(int roomId, string username, DateTime startTime, DateTime endTime, DateTime createTime, int createdBy)
	{
		var userId = getUserId(username);
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
	private long getUserId(string username)
	{
		var db = new checkITContext();
		var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
		long id = -1;
		if (user != null)
		{
			id = user.Id;
		}
		return id;
	}
}