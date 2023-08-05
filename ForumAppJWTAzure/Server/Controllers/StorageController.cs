using ForumAppJWTAzure.Shared.Helpers;
using ForumAppJWTAzure.Shared.Models;
using Newtonsoft.Json;
using System.Security.AccessControl;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IApplogService appLogService;
        private readonly UserManager<ApplicationUser> userManager;
        private string classFileName = "StorageController";
        public StorageController(IConfiguration configuration, IApplogService appLogService, UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.appLogService = appLogService;
            this.userManager = userManager;
        }

        // GET: api/<StorageController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StorageController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("uploadprofilepic")]
        public async Task<ActionResult<StorageViewModel>> Post(StorageViewModel model)
        {
            AppLog log = new() { FileName = classFileName, Method = "Post", Project = Lookups.Project.Server, Message = $"Uploading {model.Guid}: {JsonConvert.SerializeObject(model)}", Severity = Lookups.Severity.Info };
            await appLogService.UploadLogEntry(log, await GetApplicationUserId());
            Console.WriteLine($"{JsonConvert.SerializeObject(model)}");
            StorageViewModel viewModel = new StorageViewModel();

            try
            {
                if (model != null && !string.IsNullOrEmpty(model.Base64) && !string.IsNullOrEmpty(model.ContainerName))
                {
                    BlobClient client = new BlobClient(this.configuration["BlobStorage:PrimaryConnectionString"], model.ContainerName, $"{model.Guid}.png");

                    log = new() { FileName = classFileName, Method = "Post", Project = Lookups.Project.Server, Message = "Created Client", Severity = Lookups.Severity.Info };
                    await appLogService.UploadLogEntry(log, await GetApplicationUserId());

                    var encodedImage = model.Base64.Contains(",") ? model.Base64.Split(',')[1] : model.Base64;
                    var decodedImage = Convert.FromBase64String(encodedImage);

                    log = new() { FileName = classFileName, Method = "Post", Project = Lookups.Project.Server, Message = "Decoded Image", Severity = Lookups.Severity.Info };
                    await appLogService.UploadLogEntry(log, await GetApplicationUserId());

                    using (var image = Image.Load(decodedImage))
                    {
                        int height = image.Height;
                        int width = image.Width;

                        if (height > 300 || width > 300)
                        {
                            int heightDiff = height - 300;
                            int widthDiff = width - 300;

                            if (heightDiff > widthDiff)
                            {
                                image.Mutate(x => x.Resize(0, 300));
                            }
                            else
                            {
                                image.Mutate(x => x.Resize(300, 0));
                            }
                        }

                        using Stream stream = new MemoryStream();

                        image.SaveAsPng(stream);

                        log = new() { FileName = classFileName, Method = "Post", Project = Lookups.Project.Server, Message = "Saved Image", Severity = Lookups.Severity.Info };
                        await appLogService.UploadLogEntry(log, await GetApplicationUserId());

                        stream.Position = 0; // The position needs to be reset.

                        await client.UploadAsync(stream, true);

                        log = new() { FileName = classFileName, Method = "Post", Project = Lookups.Project.Server, Message = "Uploaded Image", Severity = Lookups.Severity.Info };
                        await appLogService.UploadLogEntry(log, await GetApplicationUserId());

                        viewModel = new()
                        {
                            Uri = client.Uri.ToString(),
                        };
                        
                        return this.CreatedAtAction("Post", viewModel);
                    }
                }
                else
                {
                    log = new() { FileName = classFileName, Method = "Post", Project = Lookups.Project.Server, Message = "Unable to Upload Image", Severity = Lookups.Severity.Error };
                    await appLogService.UploadLogEntry(log, await GetApplicationUserId());
                }                
            }
            catch (Exception ex)
            {
                log = new() { FileName = classFileName, Method = "Post", Project = Lookups.Project.Server, Message = $"Error: {ex.Message}", Severity = Lookups.Severity.Info };
                await appLogService.UploadLogEntry(log, await GetApplicationUserId());                
            }

            return this.Problem($"Could not upload to ");
        }

        // POST api/<StorageController>
        [HttpPost]
        [Route("uploadpostpic")]
        public async Task<ActionResult<StorageViewModel>> Post(IFormFile file)
        {
            LocationViewModel locationViewModel = new();

            try
            {
                if (file != null && !string.IsNullOrEmpty(file.FileName))
                {
                    BlobClient client = new(this.configuration["BlobStorage:PrimaryConnectionString"], "post-pics", $"{Guid.NewGuid()}.png");

                    using (var image = Image.Load(file.OpenReadStream()))
                    {
                        int height = image.Height;
                        int width = image.Width;

                        if (height > 300 || width > 300)
                        {
                            int heightDiff = height - 300;
                            int widthDiff = width - 300;

                            if (heightDiff > widthDiff)
                            {
                                image.Mutate(x => x.Resize(0, 300));
                            }
                            else
                            {
                                image.Mutate(x => x.Resize(300, 0));
                            }
                        }

                        using Stream stream = new MemoryStream();

                        image.SaveAsPng(stream);
                        stream.Position = 0; // The position needs to be reset.

                        await client.UploadAsync(stream);
                        locationViewModel.Location = client.Uri.ToString();
                    }
                }

                return this.CreatedAtAction("Post", locationViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return this.Problem($"Could not upload to ");
        }

        [HttpPost]
        [Route("uploadpostpic2")]
        public async Task<ActionResult<LocationViewModel>> Post2([FromBody] UploadFileModel model)
        {
            LocationViewModel locationViewModel = new();

            try
            {
                if (model.Data != null && !string.IsNullOrEmpty(model.FileName))
                {
                    BlobClient client = new(this.configuration["BlobStorage:PrimaryConnectionString"], this.configuration["BlobStorage:post-pic-container"], $"{Guid.NewGuid()}.png");

                    using (var image = Image.Load(model.Data))
                    {
                        int height = image.Height;
                        int width = image.Width;

                        if (height > 300 || width > 300)
                        {
                            int heightDiff = height - 300;
                            int widthDiff = width - 300;

                            if (heightDiff > widthDiff)
                            {
                                image.Mutate(x => x.Resize(0, 300));
                            }
                            else
                            {
                                image.Mutate(x => x.Resize(300, 0));
                            }
                        }

                        using Stream stream = new MemoryStream();

                        image.SaveAsPng(stream);
                        stream.Position = 0; // The position needs to be reset.

                        await client.UploadAsync(stream);
                        locationViewModel.Location = client.Uri.ToString();
                    }
                }

                return this.CreatedAtAction("Post2", locationViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return this.Problem($"Could not upload to ");
        }

        // DELETE api/<StorageController>/5
        [HttpPost]
        [Route("deleteprofilepic")]
        public async Task<ActionResult<bool>> DeleteProfilePic([FromBody] StorageViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Uri) || string.IsNullOrEmpty(model.ContainerName))
                {
                    throw new InvalidDataException("image data is null or empty.");
                }

                string fileName = Path.GetFileName(model.Uri);

                BlobClient client = new BlobClient(this.configuration["BlobStorage:PrimaryConnectionString"], model.ContainerName, fileName);

                // upload image stream to blob
                await client.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
                return this.Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return this.BadRequest(false);
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
