using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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
    public IResult Post(string username, string password){
        var res = Login.CheckLogin(username, password);
        
        var statuscode = res switch {
            Login.LoginMessage.InactiveUser => StatusCodes.Status401Unauthorized,
            Login.LoginMessage.InvalidCredentials => StatusCodes.Status401Unauthorized,
            Login.LoginMessage.Success => StatusCodes.Status200OK,
            _ => StatusCodes.Status400BadRequest
        };
        
res = Login.LoginMessage.Success;

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
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email,username),
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
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
        }

        StatusCode(statuscode);

        return  Results.Unauthorized();
        }
    }
