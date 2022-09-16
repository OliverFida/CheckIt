using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;

namespace awl_raumreservierung{
	public static class Login{
		public static LoginMessage CheckLogin(string username, string password){
			var user = new checkITContext()
							.Users
							.Where(u => u.Username.ToLower() == username.ToLower())
							.FirstOrDefault();

			 // User mit username holen
			return user switch 
			{
				{Active: true} => LoginMessage.InactiveUser,
				{Passwd: var pw, Role: var role} when BC.Verify(password, pw) => (role == UserRole.Admin)?LoginMessage.SuccessAsAdmin:LoginMessage.Success,
				_ => LoginMessage.InvalidCredentials
			};
		}

		public enum LoginMessage {
			InvalidCredentials,
			Success,
			InactiveUser,
			SuccessAsAdmin
		}
	}
}
