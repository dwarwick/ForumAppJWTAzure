using ForumAppJWTAzure.Shared.Models;
using Ganss.Excel;

namespace ForumAppJWTAzure.Server.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TagService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
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

        public async Task<List<TagViewModel>> GetSuggestedTags(List<ModelOutput> outputTags)
        {
            List<Tag> tags = new ();

            foreach(var outputTag in outputTags)
            {
                Tag tag = await context.Tags.Where(x => x.Name == outputTag.PredictedLabel).FirstOrDefaultAsync() ?? new Tag();
                tags.Add(tag);
            }

            return mapper.Map<List<TagViewModel>>(tags);
        }
    }
}
