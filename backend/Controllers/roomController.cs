using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static awl_raumreservierung.Controllers.adminController;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class roomController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	private readonly ILogger<roomController> _logger;
	private readonly checkITContext ctx;

	public roomController(ILogger<roomController> logger)
	{
		_logger = logger;
		ctx = new checkITContext();
	}
	/// <summary>
	/// Liefert ein Array der Raeume in der Datenbank.
	/// </summary>
	/// <returns>Array der Raeume in der Datenbank</returns>
	[HttpGet("get")]
	[Authorize]
	public Room[] GetRooms()
	{
		return ctx.Rooms.ToArray();
	}
	/// <summary>
	/// Erstellt einen neuen Raum.
	/// </summary>
	/// <param name="model">RoomModel des Raums der erstellt werden soll.</param>
	/// <returns>ReturnModel mit Statusnachricht und PublicRoom, wenn erfolgreich, in "Data".</returns>
	[HttpPut("add")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Add(CreateRoomModel model)
	{
		Room room = new()
		{
			Number = model.Number,
			Name = model.Name,
			Active = model.Active
		};

		ctx.Rooms.Add(room);
		ctx.SaveChanges();
		return new ReturnModel(new StatusCodeResult(201))
		{
			Message = "Raum erfolgreich erstellt.",
			Data = room.ToPublicRoom()
		};
	}

	//(Roles = "Adminstrator")
	/// <summary>
	/// 
	/// </summary>
	/// <param name="roomId"></param>
	/// <returns></returns>
	[HttpDelete("remove")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Remove(int roomId)
	{
		var room = ctx.Rooms.Where(b => b.Id == roomId).FirstOrDefault();
		if (room != null)
		{
			ctx.Rooms.Remove(room);
			ctx.SaveChanges();
		}
		return new ReturnModel(StatusCode(StatusCodes.Status200OK))
		{
			Message = "Raum erfolgreich entfernt."
		};
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="roomId"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost("room/{roomId}/edit")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Edit(long roomId, CreateRoomModel model)
	{
		Room? room = Helpers.GetRoom(roomId);

		if (room is null)
		{
			return new ReturnModel(new StatusCodeResult(404))
			{
				Message = "Raum nicht gefunden"
			};
		}
		room.Number = model.Number;
		room.Name = model.Name;
		ctx.SaveChanges();
		return new ReturnModel(StatusCode(StatusCodes.Status200OK))
		{
			Data = room.ToPublicRoom(),
			Message = "Raum erfolgreich editiert."
		};
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="roomId"></param>
	/// <returns></returns>
	[HttpPost("room/{roomId}/activate")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Activate(long roomId)
	{
		Room? room = Helpers.GetRoom(roomId);
		if (room is null)
		{
			return new ReturnModel(new StatusCodeResult(404))
			{
				Message = "Raum wurde nicht gefunden."
			};
		}
		room.Active = true;
		ctx.SaveChanges();
		return new ReturnModel(new StatusCodeResult(201))
		{
			Data = room.ToPublicRoom(),
			Message = "Raum erfolgreich aktiviert!"
		};

	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="roomId"></param>
	/// <returns></returns>
	[HttpPost("room/{roomId}/deactivate")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Deactivate(long roomId)
	{
		Room? room = Helpers.GetRoom(roomId);
		if (room is null)
		{
			return new ReturnModel(new StatusCodeResult(404))
			{
				Message = "Raum wurde nicht gefunden."
			};
		}
		room.Active = false;
		ctx.SaveChanges();
		return new ReturnModel(new StatusCodeResult(200))
		{
			Data = room.ToPublicRoom(),
			Message = "Raum erfolgreich deaktiviert!"
		};
	}
}
