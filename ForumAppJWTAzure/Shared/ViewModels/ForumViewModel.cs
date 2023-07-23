namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class ForumViewModel : BaseViewModel
    {
        public string? Title { get; set; }

        public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();

        public string? PostText { get; set; }

        public List<ForumTagViewModel> ForumTags { get; set; } = new List<ForumTagViewModel>();

        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
    }
}
