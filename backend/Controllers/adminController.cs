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

    [HttpPost()]
    public User Post(string username, string password){
        return new User(); //ohne pw und logondaten
    }
}
