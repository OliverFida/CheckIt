using awl_raumreservierung.classes;
using awl_raumreservierung.core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace awl_raumreservierung.Controllers;

/// <summary>
///
/// </summary>
[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class loginController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	/// <summary>
	///
	/// </summary>
	private readonly ILogger<loginController> _logger;
	private readonly Helpers helper;
	private readonly checkITContext ctx;

	/// <summary>
	///
	/// /// </summary>
	/// <param name="logger"></param>
	/// <param name="_context"></param>
	public loginController(ILogger<loginController> logger, checkITContext _context) {
		_logger = logger;
		ctx = _context;
		helper = new Helpers(ctx);
	}

	/// <summary>
	/// Prüft den Login und stellt einen JWT bereit
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost(Name = "login")]
	[ProducesResponseType(401)]
	[ProducesResponseType(400)]
	[ProducesResponseType(200)]

	public IResult Post(LoginUserModel model) {
		var res = Login.CheckLogin(model.Username, model.Password, ctx);

		var statuscode = res switch {
			Login.LoginMessage.InactiveUser => StatusCodes.Status401Unauthorized,
			Login.LoginMessage.InvalidCredentials => StatusCodes.Status401Unauthorized,
			Login.LoginMessage.Success or Login.LoginMessage.SuccessAsAdmin => StatusCodes.Status200OK,
			_ => StatusCodes.Status400BadRequest
		};

		Response.StatusCode = statuscode;

		if(res == Login.LoginMessage.InactiveUser) {
			return Results.BadRequest(new { value = string.Empty, message = "Benutzerkonto deaktiviert! Kontaktieren Sie einen Administrator." });
		}

		if(res == Login.LoginMessage.InvalidCredentials) {
			return Results.BadRequest(new { value = string.Empty, message = "Ungültige Benutzerdaten angegeben!" });
		}

		if(res is Login.LoginMessage.Success or Login.LoginMessage.SuccessAsAdmin) {
			var claims = new List<Claim>();
			if(res == Login.LoginMessage.SuccessAsAdmin) {
				claims.Add(new Claim(ClaimTypes.Role, "Admin"));
			}

			claims.Add(new Claim(ClaimTypes.Role, "User"));

			var issuer = Globals.AppBuilder.Configuration["Jwt:Issuer"];
			var audience = Globals.AppBuilder.Configuration["Jwt:Audience"];
			var key = Encoding.ASCII.GetBytes(Globals.AppBuilder.Configuration["Jwt:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(
					new[]
					{
						new Claim("Id", Guid.NewGuid().ToString()),
						new Claim(JwtRegisteredClaimNames.Sub, model.Username),
						new Claim(JwtRegisteredClaimNames.Email, model.Username),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					}.Union(claims)
				),
				Expires = DateTime.UtcNow.AddMinutes(5),
				Issuer = issuer,
				Audience = audience,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var stringToken = tokenHandler.WriteToken(token);
			return Results.Ok(
				new {
					Role = res switch {
						Login.LoginMessage.SuccessAsAdmin => UserRole.Admin,
						Login.LoginMessage.Success => UserRole.User,
						_ => throw new Exception()
					},
					stringToken
				}
			);
		}

		return Results.Unauthorized();
	}

	/// <summary>
	/// Macht nichts
	/// </summary>
	[HttpPost("/logout")]
	[Authorize]
	public void PostLogout() {
		//   var authUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
