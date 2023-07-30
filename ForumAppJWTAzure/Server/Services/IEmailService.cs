using ForumAppJWTAzure.Shared.Models;

namespace ForumAppJWTAzure.Server.Services
{
    public interface IEmailService
    {
        public Task SendEmail(SendEmailModel model);
        public Task SendEmailVerificationMessageAsync(ApplicationUser applicationUserViewModel, string token);
    }
}
