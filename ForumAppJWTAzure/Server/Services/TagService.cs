using ForumAppJWTAzure.Shared.Models;
using Ganss.Excel;

namespace ForumAppJWTAzure.Server.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_1 { get; }
        public PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_2 { get; }
        public PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_3 { get; }
        public PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_4 { get; }
        public PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_5 { get; }

        public TagService(ApplicationDbContext context, IMapper mapper,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_1,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_2,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_3,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_4,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_5)
        {
            this.context = context;
            this.mapper = mapper;

            this.PredictTagsModel_1 = PredictTagsModel_1;
            this.PredictTagsModel_2 = PredictTagsModel_2;
            this.PredictTagsModel_3 = PredictTagsModel_3;
            this.PredictTagsModel_4 = PredictTagsModel_4;
            this.PredictTagsModel_5 = PredictTagsModel_5;
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

        public async Task<List<ModelOutput>> MakePredictions(ModelInput input)
        {
            List<Task> tasks = new();
            List<ModelOutput> modelOutputs = new();

            tasks.Add(Task.Run(() => { modelOutputs.Add(PredictTagsModel_1.Predict(modelName: "PredictTagsModel_1", input)); }));
            tasks.Add(Task.Run(() => { modelOutputs.Add(PredictTagsModel_2.Predict(modelName: "PredictTagsModel_2", input)); }));
            tasks.Add(Task.Run(() => { modelOutputs.Add(PredictTagsModel_3.Predict(modelName: "PredictTagsModel_3", input)); }));
            tasks.Add(Task.Run(() => { modelOutputs.Add(PredictTagsModel_4.Predict(modelName: "PredictTagsModel_4", input)); }));
            tasks.Add(Task.Run(() => { modelOutputs.Add(PredictTagsModel_5.Predict(modelName: "PredictTagsModel_5", input)); }));

            await Task.WhenAll(tasks);

            return modelOutputs;
        }
    }
}
