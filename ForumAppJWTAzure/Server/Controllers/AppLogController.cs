using ForumAppJWTAzure.Client;
using ForumAppJWTAzure.Server.Data;
using ForumAppJWTAzure.Shared.ViewModels;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppLogController : SupplementalController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        
        private readonly IApplogService appLogService;

        public AppLogController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IApplogService applogService) : base(userManager)
        {
            this.context = context;
            this.mapper = mapper;            
            appLogService = applogService;
        }

        // GET: api/Forum
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<AppLogViewModel>>> Get()
        {
            List<AppLog>? logs = null;

            if (this.context.AppLogs == null)
            {
                return this.NotFound();
            }

            try
            {
                logs = await this.context.AppLogs.OrderByDescending(x => x.CreatedDate).Include(x => x.CreatedBy).ToListAsync();
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to get app logs.\n\n{ex}");
            }

            return this.Ok(this.mapper.Map<List<AppLogViewModel>>(logs));
        }

        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AppLogViewModel>> Post(AppLogViewModel logViewModel)
        {
            try
            {
                AppLog appLog = this.mapper.Map<AppLog>(logViewModel);

                appLog =  await appLogService.UploadLogEntry(appLog, await base.GetApplicationUserId());

                return this.CreatedAtAction("Post", new { id = appLog.Id }, logViewModel);
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to post app log.\n\n{ex}");
            }
        }

        // DELETE: api/Forum/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppLog(int id)
        {
            if (this.context.Forums == null)
            {
                return this.NotFound();
            }

            AppLog? log = await this.context.AppLogs.FindAsync(id);
            if (log == null)
            {
                return this.NotFound();
            }

            this.context.AppLogs.Remove(log);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        
    }
}
