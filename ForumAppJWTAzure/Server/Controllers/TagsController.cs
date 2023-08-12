using ForumAppJWTAzure.Shared.Models;
using System.Net;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<StorageController> logger;
        private readonly ITagService tagService;

        public TagsController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, ILogger<StorageController> logger, ITagService tagService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.logger = logger;
            this.tagService = tagService;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            if (this.context.Tags == null)
            {
                return this.NotFound();
            }

            return await this.context.Tags.OrderBy(x => x.Name).ToListAsync();
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            if (this.context.Tags == null)
            {
                return this.NotFound();
            }

            var tag = await this.context.Tags.FindAsync(id);

            if (tag == null)
            {
                return this.NotFound();
            }

            return tag;
        }

        // PUT: api/Tags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return this.BadRequest();
            }

            this.context.Entry(tag).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.TagExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/Tags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(TagViewModel tagViewModel)
        {
            if (this.context.Tags == null)
            {
                return this.Problem("Entity set 'ApplicationDbContext.Tags'  is null.");
            }

            Tag tag = this.mapper.Map<Tag>(tagViewModel);
            this.context.Tags.Add(tag);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }

        // POST api/<StorageController>
        [HttpPost]
        [Route("uploadmltags")]
        public async Task<ActionResult<IList<UploadResult>>> UploadMlTags([FromForm] IEnumerable<IFormFile> files)
        {
            var maxAllowedFiles = 5;
            long maxFileSize = 1024 * 1024 * 15;
            var filesProcessed = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            List<UploadResult> uploadResults = new();

            string applicationUserId = await GetApplicationUserId();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();

                var untrustedFileName = file.FileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        logger.LogInformation("{FileName} length is 0 (Err: 1)",
                            trustedFileNameForDisplay);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        logger.LogInformation("{FileName} of {Length} bytes is " +
                            "larger than the limit of {Limit} bytes (Err: 2)",
                            trustedFileNameForDisplay, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            uploadResult.NumberTagsUploaded = await tagService.BulkUploadTagsFromXLSX(file, applicationUserId);
                            uploadResult.StoredFileName = file.FileName;
                            uploadResult.Uploaded = true;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                                trustedFileNameForDisplay, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }

                    filesProcessed++;
                }
                else
                {
                    logger.LogInformation("{FileName} not uploaded because the " +
                        "request exceeded the allowed {Count} of files (Err: 4)",
                        trustedFileNameForDisplay, maxAllowedFiles);
                    uploadResult.ErrorCode = 4;
                }

                uploadResults.Add(uploadResult);
            }

            return new CreatedResult(resourcePath, uploadResults);
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            if (this.context.Tags == null)
            {
                return this.NotFound();
            }

            var tag = await this.context.Tags.FindAsync(id);
            if (tag == null)
            {
                return this.NotFound();
            }

            this.context.Tags.Remove(tag);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool TagExists(int id)
        {
            return (this.context.Tags?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<string> GetApplicationUserId()
        {
            if (this.User.Identity != null)
            {
                var userIdentity = (ClaimsIdentity)this.User.Identity;
                var claims = userIdentity.Claims;
                var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

                var id = claims.FirstOrDefault(x => x.Type == "uid");

                if (id != null)
                {
                    var user = await this.userManager.FindByIdAsync(id.Value);

                    return id.Value;
                }
            }

            return "system";
        }
    }
}
