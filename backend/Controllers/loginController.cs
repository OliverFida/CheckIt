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
    public test Post(string username, string password){
        
        return test.a; //ohne pw und logondaten
    }

    public enum test
    {
        a,b,c,d,e
    }
}
