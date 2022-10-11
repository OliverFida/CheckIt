using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
	private readonly Helpers helper;

	private readonly checkITContext ctx;
	/// <summary>
	/// 
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="_context"></param>
	public roomsController(ILogger<roomsController> logger, checkITContext _context)
	{
		_logger = logger;
		ctx = _context;
		helper = new Helpers(ctx);
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
		if (helper.DoesRoomExist(roomId))
		{
			var room = helper.GetRoom(roomId);
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

		if (!helper.DoesRoomExist(roomId))
		{
			return new ReturnModel(new StatusCodeResult(404))
			{
				Message = "Raum nicht gefunden"
			};
		}
		Room room = helper.GetRoom(roomId);
		room.Number = model.Number;
		room.Name = model.Name;
		ctx.Rooms.Update(room);
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
		if (!helper.DoesRoomExist(roomId))
		{
			return new ReturnModel(new StatusCodeResult(404))
			{
				Message = "Raum wurde nicht gefunden."
			};
		}
		Room room = helper.GetRoom(roomId);
		room.Active = true;
		ctx.Rooms.Update(room);
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
		if (!helper.DoesRoomExist(roomId))
		{
			return new ReturnModel(new StatusCodeResult(404))
			{
				Message = "Raum wurde nicht gefunden."
			};
		}
		Room room = helper.GetRoom(roomId);
		room.Active = false;
		ctx.Rooms.Update(room);
		ctx.SaveChanges();
		return new ReturnModel(new StatusCodeResult(200))
		{
			Data = room.ToPublicRoom(),
			Message = "Raum erfolgreich deaktiviert!"
		};
	}
}
