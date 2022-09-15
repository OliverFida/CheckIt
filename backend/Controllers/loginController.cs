using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
public class loginController : ControllerBase
{
    private readonly ILogger<loginController> _logger;

    public loginController(ILogger<loginController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "login")]
    public Login.LoginMessage Post(string username, string password){
        return Login.CheckLogin(username, password);
    }

    public enum test
    {
        a,b,c,d,e
    }
}
