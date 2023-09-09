namespace ForumAppJWTAzure.Client.Services
{
    public class NotificationService : BaseHttpService, INotificationService
    {
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

        public override Task<bool> Put<T>(T value, string endPoint) 
        { 
            return base.Put<T>(value, endPoint);
        }
    }
}
