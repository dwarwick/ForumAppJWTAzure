using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumAppJWTAzure.Server.Controllers
{
    
    public class SupplementalController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public SupplementalController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> GetApplicationUserId()
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
