namespace ForumAppJWTAzure.Server.Services
{
    public interface IApplogService
    {
        Task<AppLog> UploadLogEntry(AppLog appLog, string applicationUserId);
    }
}
