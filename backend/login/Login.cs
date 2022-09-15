using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Http.Logging;

namespace awl_raumreservierung{
	public static class Login{
		public static async LoginMessage CheckLogin(string username, string password){
			var user = new Class()
							.User
							.Where(u => u.Username == username)
							.FirstOrDefault();

			 // User mit username holen
			return user switch 
			{
				{Active: 1} => LoginMessage.InactiveUser,
				{Passwd: var pw} when BC.Verify(password, pw) => LoginMessage.Success,
				_ => LoginMessage.InvalidCredentials
			};
		}

		public enum LoginMessage {
			InvalidCredentials,
			Success,
			InactiveUser

		}
	}
}
