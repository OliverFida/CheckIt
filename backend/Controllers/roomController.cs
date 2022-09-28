using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static awl_raumreservierung.Controllers.adminController;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
public class roomController : ControllerBase
{
    private readonly ILogger<roomController> _logger;

    public roomController(ILogger<roomController> logger)
    {
        _logger = logger;
    }

	[HttpGet("get")]
	[Authorize]
	public Room[] getRooms()
	{
		var db = new checkITContext();
		return db.Rooms.ToArray();
	}
	[HttpPut("add")]
	[Authorize(Roles = "Adminstrator")]
	public StatusCodeResult add(string roomNr, string roomName)
	{
		var db = new checkITContext();
		var room = new Room(roomNr,roomName);

      db.Rooms.Add(room);
		db.SaveChanges();
		return StatusCode(StatusCodes.Status201Created);
	}

	//(Roles = "Adminstrator")
	[HttpDelete("remove")]
	[Authorize(Roles = "Adminstrator")]
	public StatusCodeResult Put(int roomId)
	{
		var db = new checkITContext();
		var room = db.Rooms.Where(b => b.Id == roomId).FirstOrDefault();
		if (room != null)
		{
			db.Rooms.Remove(room);
			db.SaveChanges();
		}
		return StatusCode(StatusCodes.Status200OK);
	}
	[HttpPost("edit")]
	[Authorize(Roles = "Adminstrator")]
	public StatusCodeResult edit(int roomId, string newNr, string newName)
	{
		var db = new checkITContext();
		var room = db.Rooms.Where(b => b.Id == roomId).FirstOrDefault();
		if (room != null)
		{
			room.Number = newNr;
			room.Name = newName;
			db.SaveChanges();
		}
		return StatusCode(StatusCodes.Status200OK);
	}
}
