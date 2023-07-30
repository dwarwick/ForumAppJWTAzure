namespace ForumAppJWTAzure.Shared.Models
{
    public class SendEmailModel
    {
        public string? ToEmail { get; set; }
        public string? FromEmail { get; set; }
        public string? Subject { get; set; }
        public string? TextBody { get; set; }
        public string? HtmlBody { get; set; }
        public string? Tag { get; set; }
        public int TemplateId { get; set; }

        public EmailTemplateModel? EmailTemplate { get; set; }

        public string? ApplicationUserId { get; set; }
    }
}
