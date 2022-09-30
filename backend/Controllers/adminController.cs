using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
public class adminController : ControllerBase
{
	private readonly ILogger<adminController> _logger;

    private checkITContext ctx;

	public adminController(ILogger<adminController> logger)
	{
		_logger = logger;
        ctx = new checkITContext();
	}

	[HttpPut("user")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Put(CreateUserModel model)
	{
		try
		{
			if (model.username.IsNullOrWhiteSpace())
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
                return new ReturnModel()
				{
					status = 400,
					statusMessage = "error",
					message = "Das Feld Benutzername darf nicht leer sein!"
				};
			}

			var existingUser = Helpers.GetUser(model.username);

			if (existingUser != null)
			{
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new ReturnModel()
				{
					status = 400,
					statusMessage = "error",
					message = "Ein Benutzer mit diesem Benutzernamen existiert bereits!"
				};
			}


			ctx.Add(new User {
				Username = model.username,
				Firstname = model.firstname,
				Lastname = model.lastname,
				Passwd = model.password,
				Lastchange = DateTime.Now,
				Role = model.rolle,
				Active = true
			});
			ctx.SaveChanges();
            Response.StatusCode = StatusCodes.Status201Created;

            return new ReturnModel()
			{
				message = $"Benutzer {model.username} erfolgreich angelegt!",
				data = Helpers.GetUser(model.username).ToPublicUser()
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}

	[HttpPatch("user/{username}")]
	[Authorize(Roles = "Admin")]
	public ReturnModel UpdateUser(string username, UpdateUserModel model)
	{
		try
		{
			var user = Helpers.GetUser(username);

			if(user is null)
			{
				Response.StatusCode = 404;
				return new ReturnModel
				{
					status = 404,
					statusMessage = "error",
					message = "Benutzer konnte nicht gefunden werden!"
				};
			}

			user.Firstname = model.firstname;
			user.Lastname = model.lastname;
			user.Role = model.rolle;
			user.Lastchange = DateTime.Now;

			ctx.Users.Update(user);
			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel()
			{
				message = $"Benutzer {username} erfolgreich bearbeitet!",
				data = user.ToPublicUser()
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}

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
				return new ReturnModel(new StatusCodeResult(404))
				{
					message = $"Benutzer {username} wurde nicht gefunden!"
				};
			}
            ctx.Users.Remove(user);
			ctx.SaveChanges();

			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel()
			{
				message = $"Benutzer {username} erfolgreich gelöscht!"
			};

		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}

	}

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
					status = 404,
					statusMessage = "error",
					message = $"Benutzer {username} wurde nicht gefunden!"
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
				status = 200,
				message = $"Benutzer {username} wurde {(newStatus ? "re" : "de")}aktiviert!",
				data = user.ToPublicUser()
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}

	[HttpGet("users")]
	[Authorize(Roles = "Admin")]
	public PublicUser[] Get()
	{
		try
		{
			var users = new checkITContext().Users.Select(u => u.ToPublicUser()).ToArray();
			return users;
		}
		catch(Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);
			Response.StatusCode = StatusCodes.Status500InternalServerError;
			return new PublicUser[0];
		}
	}

	[HttpGet("roles")]
	[Authorize(Roles = "Admin")]
	public Dictionary<int, string> GetRoles()
	{
		try
		{
            return Enum.GetValues(typeof(UserRole))
				.Cast<UserRole>()
				.ToDictionary(t => (int)(object)t, t => t.ToString());
        }
		catch(Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new Dictionary<int, string>();
        }
	}

	[HttpPatch("user/{username}/password")]
	[Authorize(Roles = "Admin")]
	public ReturnModel ChangePassword(string username, string password)
	{
		try
		{
			var isAdmin = User.IsInRole("Admin");
			var user = Helpers.GetUser(username);

			if (user is null)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				return new ReturnModel(new StatusCodeResult(404))
				{
					message = $"Benutzer {username} wurde nicht gefunden!"
				};
			}

			user.Passwd = password;
			user.Lastchange = DateTime.Now;
			ctx.Users.Update(user);
			ctx.SaveChanges();
			Response.StatusCode = StatusCodes.Status200OK;
			return new ReturnModel()
			{
				message = $"Passwort von Benutzer {username} erfolgreich geändert!"
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}
}
