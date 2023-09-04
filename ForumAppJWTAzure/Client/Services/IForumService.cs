namespace ForumAppJWTAzure.Client.Services
{
    public interface IForumService 
    {
        Task<Response<ForumViewModel>> CreateNewForum(ForumViewModel model);

        Task<Response<List<ForumViewModel>>> GetAllForums();

        Task<Response<T>> Create<T>(T model, string endPoint);

        Task<bool> DeleteAsync<T>(T model, string endPoint);
    }
}
