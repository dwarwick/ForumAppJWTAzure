namespace ForumAppJWTAzure.Client.Services
{
    public interface ITagService
    {
        Task<Response<List<TagViewModel>>> GetAllTags();

        Task<Response<T>> Create<T>(T model, string endPoint);

        Task<Response<List<T>>> Get<T>(string endPoint);

        Task<Response<List<TagViewModel>>> GetSuggestedTags(ForumViewModel forum);
    }
}
