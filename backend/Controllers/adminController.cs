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
			if (String.IsNullOrWhiteSpace(model.username))
			{
				StatusCode(StatusCodes.Status404NotFound);
				return new ReturnModel()
				{
					status = 400,
					statusMessage = "error",
					message = "Das Feld Benutzername darf nicht leer sein!"
				};
			}

			var existingUser = UserHelpers.GetUser(model.username);

			if (existingUser != null)
			{
				StatusCode(StatusCodes.Status400BadRequest);
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
			StatusCode(StatusCodes.Status201Created);

			return new ReturnModel()
			{
				message = $"Benutzer {model.username} erfolgreich angelegt!"
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			StatusCode(StatusCodes.Status400BadRequest);
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}

	[HttpPost("users/{username}")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Post(string username, string firstname, string lastname, UserRole rolle)
	{
		try
		{
			var context = new checkITContext();

			var user = context.Users.Where(u => u.Username == username).First();

			user.Firstname = firstname;
			user.Lastname = lastname;
			user.Role = rolle;
			user.Lastchange = DateTime.Now;

			context.SaveChanges();

			StatusCode(StatusCodes.Status200OK);
			return new ReturnModel()
			{
				message = $"Benutzer {username} erfolgreich bearbeitet!"
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			StatusCode(StatusCodes.Status400BadRequest);
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}

	[HttpDelete("users/{username}")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Delete(string username)
	{
		try
		{
			var user = UserHelpers.GetUser(username);

			if (user is null)
			{
				StatusCode(StatusCodes.Status404NotFound);
				return new ReturnModel()
				{
                    status = 404,
					message = $"Benutzer {username} wurde nicht gefunden!"
				};
			}

            ctx.Users.Remove(user);
			ctx.SaveChanges();

			StatusCode(StatusCodes.Status200OK);
			return new ReturnModel()
			{
				message = $"Benutzer {username} erfolgreich gelöscht!"
			};

		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			StatusCode(StatusCodes.Status400BadRequest);
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}

	}

	[HttpPost("users/{username}/activate")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Post(string username)
	{
		try
		{
			var user = ctx.Users.Where(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault();

			if (user is null)
			{
				StatusCode(StatusCodes.Status404NotFound);
				return new ReturnModel()
				{
					status = 404,
					statusMessage = "error",
					message = $"Benutzer {username} wurde nicht gefunden!"
				};
			}

            bool newStatus = !user.Active;
            user.Active = newStatus;
			ctx.SaveChanges();

			StatusCode(StatusCodes.Status200OK);
			return new ReturnModel()
			{
				status = 200,
				message = $"Benutzer {username} wurde {(newStatus ? "re" : "de")}aktiviert!"
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			StatusCode(StatusCodes.Status400BadRequest);
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
		var users = new checkITContext().Users.Select(u => new PublicUser(u)).ToArray();
		return users;
	}

	[HttpPost("users/{username}/password")]
	[Authorize]
	public ReturnModel PostChangePassword(string username, string password)
	{
		try
		{

			var context = new checkITContext();
			var isAdmin = User.FindAll(ClaimTypes.Role).Any(c => c is { Type: ClaimTypes.Role } and { Value: "Admin" });
			var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var actualUsername = isAdmin ? username : authUsername;

			var user = context.Users.Where(u => u.Username == username).FirstOrDefault();

			if (user is null)
			{
				StatusCode(StatusCodes.Status400BadRequest);
				return new ReturnModel()
				{
					message = $"Benutzer {username} wurde nicht gefunden!"
				};
			}

			user.Passwd = password;

			context.SaveChanges();
			StatusCode(StatusCodes.Status200OK);

			return new ReturnModel()
			{
				message = $"Passwort von Benutzer {username} erfolgreich geändert!"
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			StatusCode(StatusCodes.Status400BadRequest);
			return new ReturnModel()
			{
				status = 400,
				statusMessage = "error",
				message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}
}
