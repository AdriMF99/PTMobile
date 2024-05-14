namespace PTMobile.Models
{
    public class Project
    {
        public string? Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string? ApiKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
