namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class BaseViewModel
    {
        [Key]
        public int Id { get; set; }

        public ApplicationUserViewModel? CreatedBy { get; set; }

        public string? CreatedById { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
