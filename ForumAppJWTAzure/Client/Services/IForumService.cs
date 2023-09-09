namespace ForumAppJWTAzure.Client.Services
{
    public interface IForumService 
    {
        Task<Response<ForumViewModel>> CreateNewForum(ForumViewModel model);

        Task<Response<List<ForumViewModel>>> GetAllForums();

        Task<Response<ForumViewModel>> GetForum(int forumId);

        Task<Response<FollowedForumViewModel>> FollowAsync(FollowedForumViewModel model, string endPoint);

        Task<bool> UnfollowAsync<T>(T model, string endPoint);
    }
}
