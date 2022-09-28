using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
public class adminController : ControllerBase
{
	private readonly ILogger<adminController> _logger;

	public adminController(ILogger<adminController> logger)
	{
		_logger = logger;
	}

	[HttpPut("users/{username}")]
	[Authorize(Roles = "Admin")]
	public ReturnModel Put(string username, string firstname, string lastname, UserRole rolle, string password)
	{

		try
		{
			var ctx = new checkITContext();

			if (String.IsNullOrWhiteSpace(username))
			{
				StatusCode(StatusCodes.Status400BadRequest);
				return new ReturnModel()
				{
					status = 400,
					statusMessage = "error",
					message = "Das Feld Benutzername darf nicht leer sein!"
				};
			}

			var existingUser = ctx.Users.Where(u => u.Username.ToLower() == username.ToLower()).FirstOrDefault();

			if (existingUser != null)
			{
				StatusCode(StatusCodes.Status400BadRequest);
				return new ReturnModel()
				{
					status = 400,
					statusMessage = "error",
					message = "Dieser Benutzer existiert bereits!"
				};
			}


			var newUser = new User()
			{
				Username = username,
				Firstname = firstname,
				Lastname = lastname,
				Passwd = password,
				Lastchange = DateTime.Now,
				Role = rolle,
				Active = true
			};

			ctx.Add(newUser);
			ctx.SaveChanges();
			StatusCode(StatusCodes.Status201Created);
			return new ReturnModel()
			{
				message = $"Benutzer {username} erfolgreich angelegt!"
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
			var context = new checkITContext();

			var user = context.Users.Where(u => u.Username == username).FirstOrDefault();

			if (user is null)
			{
				StatusCode(StatusCodes.Status400BadRequest);
				return new ReturnModel()
				{
                    status = 400,
					message = $"Benutzer {username} wurde nicht gefunden!"
				};
			}

			context.SaveChanges();

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

			var context = new checkITContext();

			var user = context.Users.Where(u => u.Username == username);

			if (user is null)
			{
				StatusCode(StatusCodes.Status400BadRequest);
				return new ReturnModel()
				{
					status = 400,
					statusMessage = "error",
					message = "Dieser Benutzer existiert nicht!"
				};
			}

			context.SaveChanges();

			StatusCode(StatusCodes.Status200OK);
			return new ReturnModel()
			{
				status = 200,
				message = "Dieser Benutzer existiert bereits!"
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
				StatusCode(StatusCodes.Status404NotFound);
				return new ReturnModel()
				{
					message = $"Benutzer {username} erfolgreich angelegt!"
				};
			}

			user.Passwd = password;

			context.SaveChanges();
			StatusCode(StatusCodes.Status200OK);

			return new ReturnModel()
			{
				message = $"Benutzer {username} erfolgreich angelegt!"
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
