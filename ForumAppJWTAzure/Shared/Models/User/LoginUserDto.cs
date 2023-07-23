namespace ForumAppJWTAzure.Shared.Models.User
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        [MinLength(3)]
        public string? Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "The password must be a minimum of 8 characters")]
        public string? Password { get; set; }
    }
}
