namespace ForumAppJWTAzure.Server.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public NotificationService(ApplicationDbContext context, IMapper mapper) 
        { 
            this.context = context;
            this.mapper = mapper;
        }

        public async Task AddNotification(NotificationViewModel model)
        {
            Notification mapped = mapper.Map<Notification>(model);
            context.Add(mapped);
            await context.SaveChangesAsync();
        }

    }
}
