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
	public Room[] GetRooms()
	{
		var db = new checkITContext();
		return db.Rooms.ToArray();
	}
	[HttpPut("add")]
	[Authorize(Roles = "Adminstrator")]
	public ReturnModel Add(string roomNr, string roomName)
	{
		var db = new checkITContext();
		var room = new Room(roomNr,roomName);

      db.Rooms.Add(room);
		db.SaveChanges();
		return new ReturnModel(StatusCode(StatusCodes.Status201Created))
		{
			Message = "Raum erfolgreich erstellt."
		};
	}

	//(Roles = "Adminstrator")
	[HttpDelete("remove")]
	[Authorize(Roles = "Adminstrator")]
	public ReturnModel Put(int roomId)
	{
		var db = new checkITContext();
		var room = db.Rooms.Where(b => b.Id == roomId).FirstOrDefault();
		if (room != null)
		{
			db.Rooms.Remove(room);
			db.SaveChanges();
		}
		return new ReturnModel(StatusCode(StatusCodes.Status200OK))
		{
			Message = "Raum erfolgreich entfernt."
		};
	}
	[HttpPost("edit")]
	[Authorize(Roles = "Adminstrator")]
	public ReturnModel Edit(int roomId, string newNr, string newName)
	{
		var db = new checkITContext();
		var room = db.Rooms.Where(b => b.Id == roomId).FirstOrDefault();
		if (room != null)
		{
			room.Number = newNr;
			room.Name = newName;
			db.SaveChanges();
		}
		return new ReturnModel(StatusCode(StatusCodes.Status200OK))
		{
			Message = "Raum erfolgreich editiert."
		};
	}
}
