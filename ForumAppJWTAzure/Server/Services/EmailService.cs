using ForumAppJWTAzure.Client.Services;
using ForumAppJWTAzure.Shared.Helpers;
using ForumAppJWTAzure.Shared.Models;
using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace ForumAppJWTAzure.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        private readonly IApplogService appLogService;
        private string classFileName = "EmailService";

        public EmailService(IConfiguration configuration, IApplogService applogService) 
        {
            this.configuration = configuration;
            this.appLogService = applogService;
        }

        public async Task SendEmail(SendEmailModel model)
        {

            var message = new TemplatedPostmarkMessage()
            {
                To = model.ToEmail,
                From = model.FromEmail,
                TrackOpens = true,
                TemplateId = model.TemplateId,
                TemplateModel = model.EmailTemplate,
                Tag = model.Tag,
            };

            //var imageContent = File.ReadAllBytes("test.jpg");
            //message.AddAttachment(imageContent, "test.jpg", "image/jpg", "cid:embed_name.jpg");

            var client = new PostmarkClient(configuration["Postmark:Key"]);

            try
            {
                var sendResult = await client.SendMessageAsync(message);
            }
            catch (Exception ex)
            {

                AppLog log = new() { FileName = classFileName, Method = "SendEmail", Project = Lookups.Project.Server, Message = $"Error sending email: {ex.Message}", Severity = Lookups.Severity.Error };
                await appLogService.UploadLogEntry(log, model.ApplicationUserId);
            }
        }

        public async Task SendEmailVerificationMessageAsync(ApplicationUser applicationUser, string token)
        {
            SendEmailModel model = new SendEmailModel()
            {
                FromEmail = $"noreply@{configuration["Postmark:Domain"]}",
                ToEmail = applicationUser.UserName,
                TemplateId= 32636914,
                ApplicationUserId = applicationUser.Id,
                EmailTemplate = new()
                {
                    product_name = configuration["Postmark:Domain"],
                    action_url = ApiEndpoints.ConfirmEmail(configuration["ApiUrl"] ?? "", token, applicationUser.Email),
                    name = applicationUser.DisplayName,                   
                }
            };

            await SendEmail(model);
        }
    }
}
