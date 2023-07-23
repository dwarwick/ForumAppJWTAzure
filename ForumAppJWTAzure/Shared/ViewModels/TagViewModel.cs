namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class TagViewModel : BaseViewModel
    {
        [Required]
        [MinLength(1)]
        public string? Name { get; set; }

        public ICollection<ForumTagViewModel> ForumTags { get; set; } = new List<ForumTagViewModel>();
    }
}
