namespace ForumAppJWTAzure.Client.Services
{
    public class NotificationService : BaseHttpService, INotificationService
    {
        public event EventHandler<bool> NotificationRead;

        private readonly HttpClient client;
        public NotificationService(HttpClient client, ILocalStorageService localStorage) : base(client, localStorage)
        {
            this.client = client;
        }

        public override Task<Response<List<T>>> GetAll<T>(string endPoint)
        {
            return base.GetAll<T>(endPoint);
        }

        public override Task<Response<T>> GetSingle<T>(string endPoint)
        {
            return base.GetSingle<T>(endPoint);
        }

        public override async Task<bool> Put<T>(T value, string endPoint) 
        { 
            bool success = await base.Put<T>(value, endPoint);

            NotificationRead.Invoke(this, success);

            return success;
        }
    }
}
