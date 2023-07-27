using ForumAppJWTAzure.Server.Data;

namespace ForumAppJWTAzure.Server.Services
{
    public class AppLogService : IApplogService
    {
        private readonly ApplicationDbContext context;

        public AppLogService(ApplicationDbContext context) 
        {
            this.context = context;
        }

        public async Task<AppLog> UploadLogEntry(AppLog appLog, string applicationUserId)
        {
            appLog.CreatedById = applicationUserId;

            this.context.AppLogs.Add(appLog);
            await this.context.SaveChangesAsync();

            return appLog;
        }

    }
}
