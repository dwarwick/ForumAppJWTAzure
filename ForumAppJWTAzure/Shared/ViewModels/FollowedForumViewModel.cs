using MathNet.Numerics.Providers.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class FollowedForumViewModel
    {
        public ForumViewModel? Forum { get; set; }

        public int ForumId { get; set; }

        public ApplicationUserViewModel? Follower { get; set; }

        [ForeignKey("Follower")]
        public string FollowerId { get; set; } = string.Empty;
    }
}
