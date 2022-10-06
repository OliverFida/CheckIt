using System.Configuration;
using System.Security.Claims;
using System.Threading;
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
public class userController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
    private readonly ILogger<userController> _logger;
    private readonly checkITContext ctx;

    /// <summary>
    ///
    /// </summary>
    /// <param name="logger"></param>
    public userController(ILogger<userController> logger)
    {
        ctx = new();
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

        return user.ToPublicUser();
    }

    /// <summary>
    /// Ändert das Password des angemeldeten Users
    /// </summary>
    /// <param name="password">Hash des Passworts</param>
    /// <returns></returns>
    [HttpPatch("password")]
    [Authorize]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public ReturnModel PostChangePassword([FromBody] string password)
    {
        try
        {
            var user = User.GetUser();

            user.Passwd = password;

            ctx.Users.Update(user);
            ctx.SaveChanges();

            return new ReturnModel(StatusCode(StatusCodes.Status200OK)) { Message = "Passwort erfolgreich geändert!" };
        }
        catch (Exception ex)
        {
            _logger.LogError("Fehler aufgetreten: ", ex);

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new ReturnModel(StatusCode(StatusCodes.Status200OK)) { Message = "Es ist ein Fehler aufgetreten!" };
        }
    }
}
