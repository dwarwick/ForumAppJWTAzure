using ForumAppJWTAzure.Shared.Helpers;
using ForumAppJWTAzure.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Text.Json;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MLController : SupplementalController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IApplogService appLogService;
        private readonly ITagService tagService;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_1;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_2;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_3;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_4;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_5;

        private const string classFileName = "MLController";

        public MLController(ApplicationDbContext context, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager,
            IApplogService applogService,
            ITagService tagService,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_1,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_2,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_3,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_4,
            PredictionEnginePool<ModelInput, ModelOutput> PredictTagsModel_5) : base(userManager)
        {
            this.context = context;
            this.mapper = mapper;
            appLogService = applogService;
            this.tagService = tagService;
            this.PredictTagsModel_1 = PredictTagsModel_1;
            this.PredictTagsModel_2 = PredictTagsModel_2;
            this.PredictTagsModel_3 = PredictTagsModel_3;
            this.PredictTagsModel_4 = PredictTagsModel_4;
            this.PredictTagsModel_5 = PredictTagsModel_5;
        }
        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("predicttags")]
        [Authorize]
        public async Task<ActionResult<List<TagViewModel>>> PredictHandler(ModelInput input)
        {
            List<ModelOutput> modelOutputs = new();

            List<TagViewModel> tags = new ();

            try
            {                
                ModelOutput output = PredictTagsModel_1.Predict(modelName: "PredictTagsModel_1", input);

                if(output != null && !string.IsNullOrEmpty(output?.PredictedLabel)) 
                { 
                    modelOutputs.Add(output);
                }

                output = PredictTagsModel_1.Predict(modelName: "PredictTagsModel_2", input);

                if (output != null && !string.IsNullOrEmpty(output?.PredictedLabel))
                {
                    modelOutputs.Add(output);
                }

                output = PredictTagsModel_1.Predict(modelName: "PredictTagsModel_3", input);

                if (output != null && !string.IsNullOrEmpty(output?.PredictedLabel))
                {
                    modelOutputs.Add(output);
                }
                output = PredictTagsModel_1.Predict(modelName: "PredictTagsModel_4", input);

                if (output != null && !string.IsNullOrEmpty(output?.PredictedLabel))
                {
                    modelOutputs.Add(output);
                }

                output = PredictTagsModel_1.Predict(modelName: "PredictTagsModel_5", input);

                if (output != null && !string.IsNullOrEmpty(output?.PredictedLabel))
                {
                    modelOutputs.Add(output);
                }

                if (modelOutputs == null)
                {
                    return BadRequest();
                }

                tags = await tagService.GetSuggestedTags(modelOutputs);

            }
            catch (Exception ex)
            {
                AppLog log = new() { FileName = classFileName, Method = "PredictHandler", Project = Lookups.Project.Server, Message = $"Problem predicting tags: {ex.Message}", Severity = Lookups.Severity.Error };
                await appLogService.UploadLogEntry(log, await GetApplicationUserId());
                return this.Problem($"Something Went Wrong in the {nameof(this.PredictHandler)}", statusCode: 500);
            }

            return Ok(modelOutputs);
        }
    }
}
