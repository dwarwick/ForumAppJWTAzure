using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AppSettingController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("{key}")]        
        public async Task<string> Get([FromRoute] string key)
        {
            string value = await Task.FromResult(configuration[key] ?? "");

            return value;
        }
    }
}
