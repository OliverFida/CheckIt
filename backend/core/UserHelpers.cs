namespace awl_raumreservierung {

    public class UserHelpers {


        public static User? GetUser(string username) {

            try {
                using(checkITContext ctx = new checkITContext()) {
                    return ctx.Users.Where(u => u.Username.ToLower().Trim() == username.ToLower().Trim()).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        public static User? GetUser(int id) {
            try {
                using(checkITContext ctx = new checkITContext()) {
                    return ctx.Users.Where(u => u.Id == id).FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }


    }


}