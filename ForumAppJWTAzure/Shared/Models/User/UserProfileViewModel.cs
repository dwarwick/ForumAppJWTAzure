namespace ForumAppJWTAzure.Shared.Models.User
{
    public class UserProfileViewModel : UserDto
    {
        public string? CurrentPassword { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
