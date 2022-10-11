using Microsoft.EntityFrameworkCore;

namespace awl_raumreservierung.core
{
	/// <summary>
	/// 
	/// </summary>
	public static class Globals
	{
		/// <summary>
		/// WebAppBuilder
		/// </summary>
		public static WebApplicationBuilder AppBuilder { get; set; } = null!;
		/// <summary>
		/// DBContext
		/// </summary>
		public static checkITContext DbContext { get; set; }=null!;
	}
}
