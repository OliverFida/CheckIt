using System.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
public class userController : ControllerBase
{
	private readonly ILogger<userController> _logger;

	public userController(ILogger<userController> logger)
	{
		_logger = logger;
	}

	[HttpGet("{idOrUsername}")]
	[Authorize()]
	public PublicUser GetByID(string idOrUsername)
	{
		PublicUser? publicUser;
		if (int.TryParse(idOrUsername, out int x))
		{
			publicUser = UserHelpers.GetUser(x).ToPublicUser();
		}
		else
		{
			publicUser = UserHelpers.GetUser(idOrUsername).ToPublicUser();
		}

		if (publicUser is null)
			StatusCode(StatusCodes.Status400BadRequest);

		return publicUser;
	}

	[HttpPost("password")]
	[Authorize]
	public ReturnModel PostChangePassword(string password)
	{
		try
		{
			var context = new checkITContext();
			var authUsername = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

			var user = UserHelpers.GetUser(authUsername);

			if (user is null)
			{
				StatusCode(StatusCodes.Status404NotFound);
				return new ReturnModel
				{
					status = 400,
					statusMessage = "error",
					message = "Benutzer konnte nicht gefunden werden!"
				};
			}

			user.Passwd = password;

			context.SaveChanges();
			StatusCode(StatusCodes.Status200OK);
			return new ReturnModel
			{
				message = "Passwort erfolgreich geändert!"
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
