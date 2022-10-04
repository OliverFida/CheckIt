using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static awl_raumreservierung.Controllers.adminController;

namespace awl_raumreservierung.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class roomsController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	private readonly ILogger<roomsController> _logger;
	private readonly checkITContext ctx;
	/// <summary>
	/// 
	/// </summary>
	/// <param name="logger"></param>
	public roomsController(ILogger<roomsController> logger)
	{
		_logger = logger;
		ctx = new checkITContext();
	}
	/// <summary>
	/// Liefert ein Array der Räume in der Datenbank.
	/// </summary>
	/// <returns>Array der Räume in der Datenbank</returns>
	[HttpGet()]
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
	[HttpPut()]
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


	/// <summary>
	/// Entfernt einen Raum aus der Datebank.
	/// </summary>
	/// <param name="roomId">ID des Raums der entfernt werden soll.</param>
	/// <returns>Return Model mit Statusnachricht.</returns>
	[HttpDelete("{roomId}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(StatusCodes.Status200OK)]
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
	/// Ersetzt die Raumdaten eines Raums mit neuen Daten.
	/// </summary>
	/// <param name="roomId">Die ID des Raums der bearbeitet werden soll.</param>
	/// <param name="model">Die neuen Daten für den Raum.</param>
	/// <returns>ReturnModel mit Statusnachricht und PublicRoom in 'Data', wenn erfolgreich.</returns>
	[HttpPatch("{roomId}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(StatusCodes.Status200OK)]
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
	/// Aktiviert einen Raum für Buchungen.
	/// </summary>
	/// <param name="roomId">Die ID des Raums der aktiviert werden soll.</param>
	/// <returns>ReturnModel mit Statusnachricht und PublicRoom in 'Data', wenn erfolgreich.</returns>
	[HttpPatch("{roomId}/activate")]
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
	/// Deaktiviert einen Raum für Buchungen.
	/// </summary>
	/// <param name="roomId">Die ID des Raums der deaktiviert werden soll.</param>
	/// <returns>ReturnModel mit Statusnachricht und PublicRoom in 'Data', wenn erfolgreich.</returns>
	[HttpPatch("{roomId}/deactivate")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(StatusCodes.Status200OK)]
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
