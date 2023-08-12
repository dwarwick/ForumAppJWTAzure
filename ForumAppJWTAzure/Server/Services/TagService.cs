using ForumAppJWTAzure.Shared.Models;
using Ganss.Excel;

namespace ForumAppJWTAzure.Server.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext context;

        public TagService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> BulkUploadTagsFromXLSX(IFormFile file, string applicationUserId)
        {
            ExcelMapper excelMapper = new ExcelMapper();
            List<ModelInput> tags = new();

            excelMapper.HeaderRow = false;
            using (Stream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                tags = excelMapper.Fetch<ModelInput>(memoryStream, 0).ToList();
            }


            List<Tag> existingTags = context.Tags.AsNoTracking().ToList();

            List<string> stringList = tags.Select(x => x.Tags).Distinct().ToList();

            List<Tag> NewTags = new List<Tag>();

            foreach (string tagString in stringList)
            {
                if (!existingTags.Any(x => x.Name == tagString))
                {
                    Tag tag = new()
                    {
                        Name = tagString,
                        CreatedById = applicationUserId,
                    };

                    NewTags.Add(tag);
                }
            }

            await context.AddRangeAsync(NewTags);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error uploading tags: {ex}");
            }
            return NewTags?.Count ?? 0;
        }
    }
}
