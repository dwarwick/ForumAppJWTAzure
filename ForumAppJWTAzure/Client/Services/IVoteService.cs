namespace ForumAppJWTAzure.Client.Services
{
    public interface IVoteService
    {
        Task<Response<T>> Create<T>(T model, string endPoint);

        Task<Response<List<T>>> Get<T>(string endPoint);
    }
}
