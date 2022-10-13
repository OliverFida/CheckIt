using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using awl_raumreservierung.core;

namespace awl_raumreservierung.Controllers;

/// <summary>
///
/// </summary>
[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class adminController : ControllerBase
{
	private readonly ILogger<adminController> _logger;

	private readonly checkITContext ctx;
	private readonly Helpers helper;

	/// <summary>
	///
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="_context"></param>
	public adminController(ILogger<adminController> logger, checkITContext _context)
	{
		_logger = logger;
		ctx = _context;
		helper = new Helpers(ctx);
	}

	/// <summary>
	/// Legt einen neuen User an
	/// </summary>
	/// <param name="model">Model mit Userdaten</param>
	/// <returns></returns>
	[HttpPut("user")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(400)]
	[ProducesResponseType(201)]
	public ReturnModel Put(CreateUserModel model)
	{
		try
		{
			if (model.Username.IsNullOrWhiteSpace())
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel()
				{
					Status = 400,
					StatusMessage = "error",
					Message = "Das Feld Benutzername darf nicht leer sein!"
				};
			}

			if (helper.DoesUserExist(model.Username))
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return new ReturnModel()
				{
					Status = 400,
					StatusMessage = "error",
					Message = "Ein Benutzer mit diesem Benutzernamen existiert bereits!"
				};
			}

			ctx.Add(
				new User
				{
					Username = model.Username,
					Firstname = model.FirstName,
					Lastname = model.LastName,
					Passwd = model.Password,
					Lastchange = DateTime.Now,
					Role = model.Role,
					Active = true
				}
			);
			ctx.SaveChanges();
			Response.StatusCode = StatusCodes.Status201Created;

			return new ReturnModel() { Message = $"Benutzer {model.Username} erfolgreich angelegt!", Data = helper.GetUser(model.Username ?? "").ToPublicUser() };
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
		}
	}

	/// <summary>
	/// Aktualisiert die Daten eines Users
	/// </summary>
	/// <param name="username">Zu updatender Username</param>
	/// <param name="model">Model mit Daten</param>
	/// <returns></returns>
	[HttpPatch("user/{username}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(404)]
	[ProducesResponseType(400)]
	[ProducesResponseType(200)]
	public ReturnModel UpdateUser(string username, UpdateUserModel model)
	{
		try
		{
			if (!helper.DoesUserExist(username))
			{
				Response.StatusCode = 404;
				return new ReturnModel
				{
					Status = 404,
					StatusMessage = "error",
					Message = "Benutzer konnte nicht gefunden werden!"
				};
			}
			var user = helper.GetUser(username);

			user.Firstname = model.FirstName;
			user.Lastname = model.LastName;
			user.Role = model.Role;
			user.Lastchange = DateTime.Now;

			ctx.Users.Update(user);
			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel() { Message = $"Benutzer {username} erfolgreich bearbeitet!", Data = user.ToPublicUser() };
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
		}
	}

	/// <summary>
	/// Löscht einen User
	/// </summary>
	/// <param name="username">Name des neuen Users</param>
	/// <returns></returns>
	[HttpDelete("user/{username}")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(400)]
	[ProducesResponseType(200)]
	public ReturnModel Delete(string username)
	{
		try
		{
			if (username == User.GetUsername())
			{
				throw new ArgumentException("User kann sich nicht selbst deaktivieren.");
			}

			var user = helper.GetUser(username);

			ctx.Users.Remove(user);
			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel() { Message = $"Benutzer {username} erfolgreich gelöscht!" };
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
		}
	}

	/// <summary>
	/// Aktiviert einen zuvor deaktivierten Benutzer
	/// </summary>
	/// <param name="username">Username des Users</param>
	/// <returns></returns>
	[HttpPost("user/{username}/activate")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(400)]
	[ProducesResponseType(200)]
	public ReturnModel Post(string username)
	{
		try
		{
			var user = helper.GetUser(username);

			helper.SetUserStatus(user, true);

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel()
			{
				Status = 200,
				Message = $"Benutzer {username} wurde aktiviert!",
				Data = user.ToPublicUser()
			};
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
		}
	}

	/// <summary>
	/// Deaktiviert einen Benutzer
	/// </summary>
	/// <param name="username">Username des Users</param>
	/// <returns></returns>
	[HttpPost("user/{username}/deactivate")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(400)]
	[ProducesResponseType(200)]
	public ReturnModel PostDeactivate(string username)
	{
		try
		{
			if (username == User.GetUsername())
			{
				throw new ArgumentException("User kann sich nicht selbst deaktivieren.");
			}

			var user = helper.GetUser(username);

			helper.SetUserStatus(user, false);

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel()
			{
				Status = 200,
				Message = $"Benutzer {username} wurde deaktiviert!",
				Data = user.ToPublicUser()
			};
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
		}
	}

	/// <summary>
	/// Gibt die gesamte Userliste zurück
	/// </summary>
	/// <returns>Liste aller User mit Daten</returns>
	[HttpGet("users")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(200)]
	[ProducesResponseType(500)]
	public PublicUser[] Get()
	{
		try
		{
			var users = ctx.Users.Select(u => u.ToPublicUser()).ToArray();
			return users;
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);
			Response.StatusCode = StatusCodes.Status500InternalServerError;
			return Array.Empty<PublicUser>();
		}
	}

	/// <summary>
	/// Gibt die Rollenauflistung zurück
	/// </summary>
	/// <returns>Dict aus Rollen-ID und Beschreibung</returns>
	[HttpGet("roles")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(200)]
	[ProducesResponseType(500)]
	public Dictionary<int, string> GetRoles()
	{
		try
		{
			return Enum.GetValues(typeof(UserRole)).Cast<UserRole>().ToDictionary(t => (int)(object)t, t => t.ToString());
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);
			Response.StatusCode = StatusCodes.Status500InternalServerError;
			return new Dictionary<int, string>();
		}
	}

	/// <summary>
	/// Ändert das Passwort eines Users
	/// </summary>
	/// <param name="username">Username</param>
	/// <param name="model">Model mit Hash des Passworts</param>
	/// <returns></returns>
	[HttpPatch("user/{username}/password")]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(400)]
	public ReturnModel ChangePassword(string username, PasswordModel model)
	{
		try
		{
			var isAdmin = User.IsInRole("Admin");
			var user = helper.GetUser(username);

			user.Passwd = model.Password;
			user.Lastchange = DateTime.Now;
			ctx.Users.Update(user);
			ctx.SaveChanges();
			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel() { Message = $"Passwort von Benutzer {username} erfolgreich geändert!" };
		}
		catch (Exception ex)
		{
			return this.GetErrorModel(ex);
		}
	}
#if DEBUG
	/// <summary>
	/// Yeet bookings
	/// </summary>
	[HttpPatch("/bookings/yeet")]
	[Authorize(Roles = "Admin")]
	public void DeleteBookings()
	{
		ctx.Bookings.RemoveRange(ctx.Bookings);
		ctx.SaveChanges();
	}
#endif
}
