using System.ComponentModel;

namespace awl_raumreservierung
{
    public class PublicUser
    {
        public string Username { get; set; }
        public string? FirstName { get; set; }
        public string? Lastname { get; set; }
        public DateTime? LastLogon { get; set; }
        public DateTime? LastLogonUTC
        {
            get { return LastLogon.ToUtc(); }
        }
        public DateTime? Lastchange { get; set; }
        public DateTime? LastchangeUTC
        {
            get { return Lastchange.ToUtc(); }
        }

        public UserRole? Role { get; set; }
        public bool? Active { get; set; }
        public PublicUser(awl_raumreservierung.User user)
        {
            if (user != null)
            {
                Username = user.Username;
                FirstName = user.Firstname;
                Lastname = user.Lastname;
                LastLogon = user.Lastlogon;
                Lastchange = user.Lastchange;
                Role = user.Role;
                Active = user.Active;
            }
        }
    }
}