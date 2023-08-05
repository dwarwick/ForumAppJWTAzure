namespace ForumAppJWTAzure.Server.Data
{
    public class Post : BaseModel
    {
        public string? Text { get; set; }

        public Forum? Forum { get; set; }

        public int? ForumId { get; set; }

        public Post? ReplyPost { get; set; }

        public int? ReplyPostId { get; set; }

        public string? Images { get; set; }
        public List<Vote> Votes { get; set; } = new();
    }
}
