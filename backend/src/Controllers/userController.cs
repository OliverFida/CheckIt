using awl_raumreservierung.core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

/// <summary>
///
/// </summary>
[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class userController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	private readonly Helpers helper;
	private readonly ILogger<userController> _logger;
	private readonly checkITContext ctx;

	/// <summary>
	///
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="_context"></param>
	public userController(ILogger<userController> logger, checkITContext _context) {
		ctx = _context;
		helper = new Helpers(ctx);
		_logger = logger;
	}

	/// <summary>
	/// Holt die öffentlichen Daten eines Nutzers
	/// </summary>
	/// <param name="idOrUsername"> ID oder Username</param>
	/// <returns></returns>
	[HttpGet("{idOrUsername}")]
	[Authorize()]
	[ProducesResponseType(404)]
	[ProducesResponseType(200)]
	public PublicUser? GetByID(string idOrUsername) {
		var user = int.TryParse(idOrUsername, out var x) ? helper.GetUser(x) : helper.GetUser(idOrUsername);
		return user.ToPublicUser();
	}
	/// <summary>
	/// Liefert den aktuellen Benutzer
	/// </summary>
	/// <returns></returns>
	[HttpGet("")]
	[Authorize()]
	[ProducesResponseType(404)]
	[ProducesResponseType(200)]
	public PublicUser? GetUser() => User.GetUser(helper).ToPublicUser();
	/// <summary>
	/// Ändert das Password des angemeldeten Users
	/// </summary>
	/// <param name="model">Model mit Hash des Passworts</param>
	/// <returns></returns>
	[HttpPatch("password")]
	[Authorize]
	[ProducesResponseType(404)]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	public ReturnModel PostChangePassword(PasswordModel model) {
		try {
			var user = User.GetUser(helper);

			user.PlainTextPassword = model.Password;

			ctx.Users.Update(user);
			ctx.SaveChanges();

			return new ReturnModel(StatusCode(StatusCodes.Status200OK)) { Message = "Passwort erfolgreich geändert!" };
		} catch(Exception ex) {
			_logger.LogError("Fehler aufgetreten: ", ex);

			Response.StatusCode = StatusCodes.Status400BadRequest;
			return new ReturnModel(StatusCode(StatusCodes.Status200OK)) { Message = "Es ist ein Fehler aufgetreten!" };
		}
	}
}
