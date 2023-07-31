using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<ImageController> logger;

        public ImageController(IWebHostEnvironment environment, ILogger<ImageController> logger)
        {
            this.environment = environment;
            this.logger = logger;
        }

        [HttpGet]
        [Route("doesfileexist")]
        [Authorize]
        public async Task<bool> DoesFileExist([FromQuery(Name = "path")] string path) => await Task.FromResult(System.IO.File.Exists(path));

        [HttpGet]
        [Route("doesfileexistbyuserid")]
        public async Task<bool> DoesFileExistByUserId([FromQuery(Name = "userId")] string userId)
        {
            string filePath = Path.Combine("Uploads/" + userId + ".png");

            return await Task.FromResult(System.IO.File.Exists(Path.Combine(this.environment.WebRootPath, filePath)));
        }

        [HttpPost]
        [Authorize]
        public ActionResult PostFile([FromBody] string model)
        {
            try
            {
                string? name = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? file = null;

                if (!string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(name))
                {
                    this.ProcessImage(model, name);
                    file = "Uploads/" + name + ".png";
                }

                return this.Ok(file);
            }
            catch
            {
                return this.Problem();
            }
        }

        private void ProcessImage(string croppedImage, string name)
        {
            string filePath = string.Empty;

            try
            {
                string base64 = croppedImage;
                byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);

                filePath = Path.Combine("Uploads/" + name + ".png");

                if (!Directory.Exists(Path.Combine(this.environment.WebRootPath, "Uploads")))
                {
                    Directory.CreateDirectory(Path.Combine(this.environment.WebRootPath, "Uploads"));
                }

                using (FileStream stream = new FileStream(Path.Combine(this.environment.WebRootPath, filePath), FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.Message);
            }
        }
    }
}
