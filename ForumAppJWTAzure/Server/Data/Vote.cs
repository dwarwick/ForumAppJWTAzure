namespace ForumAppJWTAzure.Server.Data
{
    public class Vote : BaseModel
    {
        public int Value { get; set; }

        public Post? Post { get; set; }

        public int PostId { get; set; }
    }
}
