using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace awl_raumreservierung.classes
{
	public static class userHelper
	{
		public static long getUserId(string username)
		{
			var db = new checkITContext();
			var user = db.Users.Where(u => u.Username == username).FirstOrDefault();
			long id = -1;
			if (user != null)
			{
				id = user.Id;
			}
			return id;
		}
		public static bool isAdmin()
		{
			var isAdmin = ControllerBase.User.FindAll(ClaimTypes.Role).Any(c => c is { Type: ClaimTypes.Role } and { Value: "Admin" });
			return isAdmin;
		}
	}
}
