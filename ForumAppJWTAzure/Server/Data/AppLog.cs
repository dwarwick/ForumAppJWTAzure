namespace ForumAppJWTAzure.Server.Data
{
    public class AppLog : BaseModel
    {
        public string? Project { get; set; }
        public string? FileName { get; set; }
        public string? Method { get; set;}
        public string? Message { get; set; }
        public string? Severity { get; set; }
    }
}
