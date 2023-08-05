namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class PostViewModel : BaseViewModel
    {
        public string? Text { get; set; }

        public ForumViewModel? Forum { get; set; }

        public int? ForumId { get; set; }

        public PostViewModel? ReplyPost { get; set; }

        public int? ReplyPostId { get; set; }
        public string? Images { get; set; }
        public List<VoteViewModel> Votes { get; set; } = new();
    }
}
