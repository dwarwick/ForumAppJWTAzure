namespace ForumAppJWTAzure.Client.Services
{
    public interface IAppLogService
    {
        public Task<Response<T>> Create<T>(T model, string endPoint);

        public Task<Response<List<T>>> Get<T>(string endPoint);

        public Task<Response<List<T>>> GetAll<T>(string endPoint);
    }
}
