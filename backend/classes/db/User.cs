using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
    public partial class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string Passwd { get; set; } = null!;
        public DateTime? Lastlogon { get; set; }
        public DateTime? Lastchange { get; set; }
        public bool Active { get; set; }
        public UserRole? Role { get; set; }
    }

    public enum UserRole {
        User,
        Admin
    }
}
