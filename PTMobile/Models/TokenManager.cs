namespace PTMobile.Models
{
    public static class TokenManager
    {
        public static string Token { get; set; }
        public static List<Project> Allprojects { get; set; }
        public static string TvCode { get; set; }
        public static string currentUser { get; set; } = "Mike";
        public static string selectedUserAdmin { get; set; }
        public static bool isFromAdmin { get; set; }
        public static bool isAdmin { get; set;}
        public static bool isGod { get; set; }
    }

}
