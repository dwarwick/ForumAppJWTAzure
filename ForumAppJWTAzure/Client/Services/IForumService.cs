namespace ForumAppJWTAzure.Client.Services
{
    public interface IForumService
    {
        Task<Response<ForumViewModel>> CreateNewForum(ForumViewModel model);

        Task<Response<List<ForumViewModel>>> GetAllForums();
    }
}
