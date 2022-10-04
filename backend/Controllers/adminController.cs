using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

	/// <summary>
	///
	/// </summary>
	/// <param name="logger"></param>
	public adminController(ILogger<adminController> logger)
	{
		_logger = logger;
		ctx = new checkITContext();
	}

	/// <summary>
	/// Legt einen neuen User an
	/// </summary>
	/// <param name="model">Model mit Userdaten</param>
	/// <returns></returns>
	[HttpPut("user")]
	[Authorize(Roles = "Admin")]
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

            var existingUser = Helpers.GetUser(model.Username ?? "");

			if (existingUser != null)
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
					Username = model.Username ?? "",
					Firstname = model.FirstName,
					Lastname = model.LastName,
					Passwd = model.Password ?? "",
					Lastchange = DateTime.Now,
					Role = model.Role,
					Active = true
				}
			);
			ctx.SaveChanges();
			Response.StatusCode = StatusCodes.Status201Created;

			return new ReturnModel() { Message = $"Benutzer {model.Username} erfolgreich angelegt!", Data = Helpers.GetUser(model.Username ?? "").ToPublicUser() };
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

	/// <summary>
	/// Aktualisiert die Daten eines Users
	/// </summary>
	/// <param name="username">Zu updatender Username</param>
	/// <param name="model">Model mit Daten</param>
	/// <returns></returns>
	[HttpPatch("user/{username}")]
	[Authorize(Roles = "Admin")]
	public ReturnModel UpdateUser(string username, UpdateUserModel model)
	{
		try
		{
			var user = Helpers.GetUser(username);

			if (user is null)
			{
				Response.StatusCode = 404;
				return new ReturnModel
				{
					Status = 404,
					StatusMessage = "error",
					Message = "Benutzer konnte nicht gefunden werden!"
				};
			}

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

	/// <summary>
	/// Löscht einen User
	/// </summary>
	/// <param name="username">Name des neuen Users</param>
	/// <returns></returns>
	[HttpDelete("user/{username}")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Delete(string username)
	{
		try
		{
			var user = Helpers.GetUser(username);

			if (user is null)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel(new StatusCodeResult(404)) { Message = $"Benutzer {username} wurde nicht gefunden!" };
			}
			ctx.Users.Remove(user);
			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel() { Message = $"Benutzer {username} erfolgreich gelöscht!" };
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

	/// <summary>
	/// Aktiviert einen zuvor deaktivierten Benutzer
	/// </summary>
	/// <param name="username">Username des Users</param>
	/// <returns></returns>
	[HttpPost("user/{username}/activate")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Post(string username)
	{
		try
		{
			var user = Helpers.GetUser(username);

            if (user is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return new ReturnModel()
                {
                    Status = 404,
                    StatusMessage = "error",
                    Message = $"Benutzer {username} wurde nicht gefunden!"
                };
            }

			bool newStatus = !user.Active;
			user.Active = newStatus;
			user.Lastchange = DateTime.Now;
			ctx.Users.Update(user);
			ctx.SaveChanges();

            Response.StatusCode = StatusCodes.Status200OK;
            return new ReturnModel()
            {
                Status = 200,
                Message = $"Benutzer {username} wurde {(newStatus ? "re" : "de")}aktiviert!",
                Data = user.ToPublicUser()
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

	/// <summary>
	/// Gibt die gesamte Userliste zurück
	/// </summary>
	/// <returns>Liste aller User mit Daten</returns>
	[HttpGet("users")]
	[Authorize(Roles = "Admin")]
	public PublicUser[] Get()
	{
		try
		{
			var users = new checkITContext().Users.Select(u => u.ToPublicUser()).ToArray();
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
	/// <param name="password">Hash des Passworts</param>
	/// <returns></returns>
	[HttpPatch("user/{username}/password")]
	[Authorize(Roles = "Admin")]
	public ReturnModel ChangePassword(string username, [FromBody] string password)
	{
		try
		{
			var isAdmin = User.IsInRole("Admin");
			var user = Helpers.GetUser(username);

			if (user is null)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel(new StatusCodeResult(404)) { Message = $"Benutzer {username} wurde nicht gefunden!" };
			}

			user.Passwd = password;
			user.Lastchange = DateTime.Now;
			ctx.Users.Update(user);
			ctx.SaveChanges();
			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel() { Message = $"Passwort von Benutzer {username} erfolgreich geändert!" };
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
}
