namespace ForumAppJWTAzure.Client.Services
{
    public interface ISearchService
    {
        event EventHandler<string> SearchCompleted;

        Task<Response<T>> Create<T>(T model, string endPoint);

        Task<Response<List<T>>> Get<T>(string endPoint);

        Task<Response<List<ForumViewModel>>> SearchBySearchTerm(SearchViewModel model);
    }
}