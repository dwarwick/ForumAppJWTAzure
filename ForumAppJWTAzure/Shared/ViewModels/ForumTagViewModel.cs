namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class ForumTagViewModel
    {
        public int ForumId { get; set; }

        public int TagId { get; set; }

        public ForumViewModel Forum { get; set; } = new();

        public TagViewModel Tag { get; set; } = new();
    }
}
