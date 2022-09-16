using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPut("users/{username}")]
    [Authorize(Roles = "Administrator")]
    public void Put(string username, string firstname, string lastname, UserRole rolle, string password){
        var context = new checkITContext();

        var newUser = new User() {
            Username = username,
            Firstname = firstname,
            Lastname = lastname,
            Passwd = password,
            Lastchange = DateTime.Now,
            Role = rolle,
            Active = true
        };

        context.Add(newUser);
        context.SaveChanges();

        StatusCode(StatusCodes.Status201Created);
    }

    [HttpDelete("users/{username}")]
    [Authorize(Roles = "Administrator")]
    public void Delete(string username){
        var context = new checkITContext();

        var user = context.Users.Where(u => u.Username == username);

        context.Add(user);
        context.SaveChanges();

        StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("users/{username}/activate")]
    [Authorize(Roles = "Administrator")]
    public void Post(string username){
        var context = new checkITContext();

        var user = context.Users.Where(u => u.Username == username);

        context.Add(user);
        context.SaveChanges();

        StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("users")]
    [Authorize(Roles = "Administrator")]
    public PublicUser[] Get(){
        var users  = new checkITContext().Users.Select(u => new PublicUser(u)).ToArray();
        return users;
    }

    [HttpPost("users/{username}/password")]
    [Authorize]
    public void PostChangePassword(string username, string password){
        var context = new checkITContext();
        var isAdmin = User.FindAll(ClaimTypes.Role).Any(c => c is {Type: ClaimTypes.Role} and  {Value: "Admin"});
        var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var actualUsername = isAdmin?username: authUsername;

        var user = context.Users.Where(u => u.Username == username).First();
        user.Passwd = password;

        context.Add(user);
        context.SaveChanges();
    }
}
