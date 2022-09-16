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