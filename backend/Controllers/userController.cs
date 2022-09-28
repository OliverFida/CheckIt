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
    public PublicUser GetByID(string idOrUsername){
        int x = 0;
        var user = new checkITContext().Users.Where(u => int.TryParse(idOrUsername, out x) ? u.Id.ToString() == idOrUsername : u.Username == idOrUsername).First();
        var publicUser = new PublicUser(user);
        return publicUser;
    }

    [HttpPost("password")]
    [Authorize]
    public void PostChangePassword(string password){
        var context = new checkITContext();
        var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = context.Users.Where(u => u.Username == authUsername).FirstOrDefault();

        if (user is null) {
            StatusCode(StatusCodes.Status404NotFound);
            return;
        }

        user.Passwd = password;

        context.SaveChanges();
        StatusCode(StatusCodes.Status200OK);
    }
}
