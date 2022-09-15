using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
public class bookingController : ControllerBase
{
    private readonly ILogger<bookingController> _logger;

    public bookingController(ILogger<bookingController> logger)
    {
        _logger = logger;
    }

    [HttpPost()]
    public User Post(string username, string password){
        return new User(); //ohne pw und logondaten
    }
}
