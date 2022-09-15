namespace awl_raumreservierung{
	public static class Login{
		public static LoginMessage CheckLogin(string username, string password){
			var user = new checkITContext()
							.Users
							.Where(u => u.Username == username)
							.FirstOrDefault();

			 // User mit username holen
			return user switch 
			{
				{Active: true} => LoginMessage.InactiveUser,
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
