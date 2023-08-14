using ForumAppJWTAzure.Shared.Models;

namespace ForumAppJWTAzure.Server.Services
{
    public interface ITagService
    {
        Task<int> BulkUploadTagsFromXLSX(IFormFile file, string applicationUserId);

        Task<List<TagViewModel>> GetSuggestedTags(List<ModelOutput> outputTags);
    }
}
