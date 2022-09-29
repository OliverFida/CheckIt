using System.Configuration;
using System.Security.Claims;
using System.Threading;
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
    public PublicUser? GetByID(string idOrUsername)
    {
        User user;
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
                return new ReturnModel(StatusCode(StatusCodes.Status404NotFound))
                {
                    message = "Benutzer konnte nicht gefunden werden!"
                };
            }

            user.Passwd = password;

            context.SaveChanges();

            return new ReturnModel(StatusCode(StatusCodes.Status200OK))
            {
                message = "Passwort erfolgreich geändert!"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ReturnModel(StatusCode(StatusCodes.Status200OK))
            {
                message = "Es ist ein Fehler aufgetreten!"
            };
        }
    }

}
