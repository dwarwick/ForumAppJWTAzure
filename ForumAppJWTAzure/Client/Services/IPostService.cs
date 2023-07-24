namespace ForumAppJWTAzure.Client.Services
{
    public interface IPostService
    {
        Task<Response<PostViewModel>> CreateNewPost(PostViewModel model);
        Task<Response<PostViewModel>> EditPost(PostViewModel model);
    }
}
