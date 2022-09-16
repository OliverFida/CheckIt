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

    [HttpPut("user")]
    [Authorize]
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

    [HttpDelete("user")]
    [Authorize]
    public void Delete(string username){
        var context = new checkITContext();

        var user = context.Users.Where(u => u.Username == username);

        context.Add(user);
        context.SaveChanges();

        StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("users")]
    [Authorize]
    public PublicUser[] Get(){
        var users  = new checkITContext().Users.Select(u => new PublicUser(u)).ToArray();
        return users;
    }

    [HttpPost("user/{username}/password")]
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
