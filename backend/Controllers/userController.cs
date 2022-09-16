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

    [HttpGet("{id}")]
    [Authorize()]
    public PublicUser GetByID(int id){
        return new PublicUser(new checkITContext().Users.Where(u => u.Id == id).First());
    }

    [HttpGet("{username}")]
    [Authorize()]
    public PublicUser GetByName(string username){
        return new PublicUser(new checkITContext().Users.Where(u => u.Username == username).First());
    }
}
