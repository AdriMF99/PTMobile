using System.Text.Json.Serialization;

namespace PTMobile.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsGod { get; set; }
    }
}
