using System.Configuration;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class userController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	private readonly ILogger<userController> _logger;
	private readonly checkITContext ctx;
	public userController(ILogger<userController> logger)
	{
		ctx = new();
		_logger = logger;
	}

	[HttpGet("{idOrUsername}")]
	[Authorize()]
	[SwaggerOperation("Holt die öffentlichen Daten eines Nutzers")]
	public PublicUser? GetByID(string idOrUsername)
	{
		User? user;
		if (int.TryParse(idOrUsername, out int x))
		{
			user = Helpers.GetUser(x);
		}
		else
		{
			user = Helpers.GetUser(idOrUsername);
		}

		if (user is null)
		{
			Response.StatusCode = StatusCodes.Status404NotFound;
			return new PublicUser(null);
		}

		return user.ToPublicUser();
	}

	[HttpPatch("password")]
	[Authorize]
	[SwaggerOperation("Ändert das Password des angemeldeten Users")]
	public ReturnModel PostChangePassword(string password)
	{
		try
		{
			var user = User.GetUser();

			if (user is null)
			{
				return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
				{
					Message = "Benutzer konnte nicht gefunden werden!"
				};
			}

			user.Passwd = password;

			ctx.Users.Update(user);
			ctx.SaveChanges();

			return new ReturnModel(StatusCode(StatusCodes.Status200OK))
			{
				Message = "Passwort erfolgreich geändert!"
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel(StatusCode(StatusCodes.Status200OK))
			{
				Message = "Es ist ein Fehler aufgetreten!"
			};
		}
	}

}
