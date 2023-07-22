namespace ForumAppJWTAzure.Server.Data
{
    public class ForumTag
    {
        public int ForumId { get; set; }

        public int TagId { get; set; }

        public Forum? Forum { get; private set; }

        public Tag? Tag { get; private set; }
    }
}
