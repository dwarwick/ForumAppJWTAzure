using System.ComponentModel.DataAnnotations.Schema;

namespace ForumAppJWTAzure.Server.Data
{
    public class FollowedForum
    {
        public Forum? Forum { get; set; }

        public int ForumId { get; set; }

        public ApplicationUser? Follower { get; set; }

        [ForeignKey("Follower")]
        public string FollowerId { get; set; } = string.Empty;
    }
}
