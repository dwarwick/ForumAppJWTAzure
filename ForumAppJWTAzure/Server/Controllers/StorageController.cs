namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public StorageController(IConfiguration configuration)
        {
            this.configuration = configuration;
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
            StorageViewModel viewModel = new StorageViewModel();

            try
            {
                if (model != null && !string.IsNullOrEmpty(model.Base64) && !string.IsNullOrEmpty(model.ContainerName))
                {
                    BlobClient client = new BlobClient(this.configuration["BlobStorage:PrimaryConnectionString"], model.ContainerName, $"{model.Guid}.png");

                    var encodedImage = model.Base64.Contains(",") ? model.Base64.Split(',')[1] : model.Base64;
                    var decodedImage = Convert.FromBase64String(encodedImage);

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
                        stream.Position = 0; // The position needs to be reset.

                        await client.UploadAsync(stream, true);
                        viewModel = new()
                        {
                            Uri = client.Uri.ToString(),
                        };
                    }
                }

                return this.CreatedAtAction("Post", viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
    }
}
