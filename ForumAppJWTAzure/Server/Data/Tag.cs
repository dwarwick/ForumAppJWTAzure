namespace ForumAppJWTAzure.Server.Data
{
    public class Tag : BaseModel
    {
        public string? Name { get; set; }

        public ICollection<Forum>? Forums { get; set; }
    }
}
