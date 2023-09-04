namespace ForumAppJWTAzure.Server.Data
{
    public class Forum : BaseModel
    {
        public string? Title { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public List<ApplicationUser> Followers { get; set; } = new List<ApplicationUser>();
    }
}
