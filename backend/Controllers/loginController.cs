using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

[ApiController]
[Route("[controller]")]
public class loginController : ControllerBase
{
    public static WebApplicationBuilder? builder;
    private readonly ILogger<loginController> _logger;

    public loginController(ILogger<loginController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "login")]
    public Login.LoginMessage Post(string username, string password){
        var res = Login.CheckLogin(username, password);
        
        var statuscode = res switch {
            Login.LoginMessage.InactiveUser => StatusCodes.Status401Unauthorized,
            Login.LoginMessage.InvalidCredentials => StatusCodes.Status401Unauthorized,
            Login.LoginMessage.Success => StatusCodes.Status200OK,
            _ => StatusCodes.Status400BadRequest
        };
        
        if (res == Login.LoginMessage.Success){
             var issuer = builder.Configuration["Jwt:Issuer"];
             var audience = builder.Configuration["Jwt:Audience"];
             var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        }

        StatusCode(statuscode);

        return Login.CheckLogin(username, password);
        }
    }
