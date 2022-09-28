namespace awl_raumreservierung {

    public static class ExtensionMethods {

        public static bool IsNullOrWhiteSpace(this string str) {
            return String.IsNullOrWhiteSpace(str);
        }

        public static PublicUser ToPublicUser(this User usr) {
            return new PublicUser(usr);
        }

    }


}