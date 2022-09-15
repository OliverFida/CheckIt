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

    [HttpPut("user")]
    public void Post(string username,string firstname,string lastname, UserRole rolle){
        StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("users")]
    public PublicUser[] Get(){
        return new awl_raumreservierung.checkITContext().Users.Select(u => new PublicUser(u)).ToArray();
    }

    public class PublicUser {
        string Username;
        string? FirstName;
        string? Lastname;
        DateTime? LastLogon;
        public PublicUser(awl_raumreservierung.User user) {
            Username = user.Username;
            FirstName = user.Firstname;
            Lastname = user.Lastname;
            LastLogon = user.Lastlogon;
         }
    }
}
