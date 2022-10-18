using awl_raumreservierung.core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.Controllers;

/// <summary>
///
/// </summary>
[ApiController]
[Route("[controller]")]
#pragma warning disable IDE1006 // Naming Styles
public class systemController : ControllerBase
#pragma warning restore IDE1006 // Naming Styles
{
	private readonly Helpers helper;
	private readonly ILogger<systemController> _logger;
	private readonly checkITContext ctx;

	/// <summary>
	///
	/// </summary>
	/// <param name="logger"></param>
	/// <param name="_context"></param>
	public systemController(ILogger<systemController> logger, checkITContext _context) {
		ctx = _context;
		helper = new Helpers(ctx);
		_logger = logger;
	}

	/// <summary>
	/// Liefert Build-Daten über das Backend
	/// </summary>
	/// <returns></returns>
	[HttpGet("version")]
	[ProducesResponseType(200)]
	public PublicVersion GetVersion() => new();
}
