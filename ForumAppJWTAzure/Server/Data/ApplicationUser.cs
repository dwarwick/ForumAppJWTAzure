namespace ForumAppJWTAzure.Server.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; } = string.Empty;

        public string Theme { get; set; } = "Dark";

        public string? ProfilePicture { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
