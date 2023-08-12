namespace ForumAppJWTAzure.Server.Services
{
    public interface ITagService
    {
        Task BulkUploadTagsFromXLSX(IFormFile file, string applicationUserId);
    }
}
