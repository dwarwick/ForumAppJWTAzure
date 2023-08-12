namespace ForumAppJWTAzure.Server.Services
{
    public interface ITagService
    {
        Task<int> BulkUploadTagsFromXLSX(IFormFile file, string applicationUserId);
    }
}
