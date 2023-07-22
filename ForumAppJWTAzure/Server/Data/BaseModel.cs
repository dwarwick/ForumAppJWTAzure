namespace ForumAppJWTAzure.Server.Data
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public ApplicationUser? CreatedBy { get; set; }

        public string? CreatedById { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
