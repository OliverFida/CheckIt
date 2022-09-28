namespace awl_raumreservierung {
  public class PublicUser {
        public string Username {get; set;}
        public string? FirstName {get; set;}
        public string? Lastname {get; set;}
        public DateTime? LastLogon {get; set;}
        public PublicUser(awl_raumreservierung.User user) {
            Username = user.Username;
            FirstName = user.Firstname;
            Lastname = user.Lastname;
            LastLogon = user.Lastlogon;
         }
    }
}