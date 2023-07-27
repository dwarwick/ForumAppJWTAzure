namespace ForumAppJWTAzure.Client.Services
{
    public class AppLogService : BaseHttpService, IAppLogService
    {
        private readonly HttpClient client;

        public AppLogService(HttpClient client, ILocalStorageService localStorage)
            : base(client, localStorage)
        {
            this.client = client;
        }

        public override Task<Response<T>> Create<T>(T model, string endPoint)
        {
            return base.Create(model, endPoint);
        }

        public override Task<Response<List<T>>> Get<T>(string endPoint)
        {
            return base.Get<T>(endPoint);
        }

        public override Task<Response<List<T>>> GetAll<T>(string endPoint)
        {            
            return base.GetAll<T>(endPoint);
        }
    }
}
