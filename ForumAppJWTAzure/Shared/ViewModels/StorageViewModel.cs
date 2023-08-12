namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class StorageViewModel
    {
        public string? Base64 { get; set; }

        public MemoryStream? MS { get; set; }       

        public string? ContentType { get; set; }

        public string? Guid { get; set; }

        public string? ContainerName { get; set; }

        public string? Uri { get; set; }
    }
}
