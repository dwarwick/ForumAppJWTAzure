namespace ForumAppJWTAzure.Server.Services
{
    public interface INotificationService
    {
        Task AddNotification(NotificationViewModel model);
    }
}