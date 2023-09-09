namespace ForumAppJWTAzure.Server.Data
{
    public class Notification : BaseModel
    {
        public string Message { get; set; } = string.Empty;

        public string Target { get; set; } = string.Empty;

        public bool Read { get; set; }
    }
}
