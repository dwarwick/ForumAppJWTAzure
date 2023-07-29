namespace ForumAppJWTAzure.Client.Services
{
    public interface IConfigurationService
    {
        public Dictionary<string, string> Configuration { get; set; }

        public Task<Response<List<T>>> Get<T>(string endPoint);

        public Task GetSettings();
    }
}
