using System.Security.Claims;

namespace awl_raumreservierung {

    public static class ExtensionMethods {

        public static bool IsNullOrWhiteSpace(this string str) {
            return String.IsNullOrWhiteSpace(str);
        }

        public static PublicUser ToPublicUser(this User usr) {
            return new PublicUser(usr);
        }

        public static User GetUser(this ClaimsPrincipal princ)
        {
            string username = princ.FindFirstValue(ClaimTypes.NameIdentifier); // Note: Nicht Single line weil er sonst bs macht
            return UserHelpers.GetUser(username);
        }

        public static string GetUsername(this ClaimsPrincipal princ)
        {
            string username = princ.FindFirstValue(ClaimTypes.NameIdentifier); // Note: Nicht Single line weil er sonst bs macht
            return username;

        }

        public static bool IsInRole(this ClaimsPrincipal princ, string roleName)
        {
            return princ.FindAll(ClaimTypes.Role).Any(c => c.Type == ClaimTypes.Role && c.Value == roleName);
        }

    }


}