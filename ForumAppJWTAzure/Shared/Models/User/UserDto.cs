namespace ForumAppJWTAzure.Shared.Models.User
{
    public class UserDto : LoginUserDto
    {
        [Required]
        public string? DisplayName { get; set; }

        [Required]
        public string? Role { get; set; }

        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
