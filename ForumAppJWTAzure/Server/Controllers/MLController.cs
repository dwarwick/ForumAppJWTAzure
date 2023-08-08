using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Text.Json;
using static ML.PredictTags;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MLController : SupplementalController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IApplogService appLogService;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> predEnginePool;

        public MLController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IApplogService applogService, PredictionEnginePool<ModelInput, ModelOutput> predictionEngine) : base(userManager)
        {
            this.context = context;
            this.mapper = mapper;
            appLogService = applogService;
            this.predEnginePool = predictionEngine;
        }
        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<List<ModelOutput>> PredictHandler(ModelInput input)
        {
            ModelOutput output = predEnginePool.Predict(modelName: "PredictTagsModel", input);
            
            if(output == null)
            {
                return BadRequest();
            }

            return Ok(output);
        }
    }
}
