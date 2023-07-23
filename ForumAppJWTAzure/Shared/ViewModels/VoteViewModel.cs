namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class VoteViewModel : BaseViewModel
    {
        public int Value { get; set; }

        public PostViewModel? Post { get; set; }

        public int PostId { get; set; }
    }
}
