using Elasticsearch.Net;
using ForumAppJWTAzure.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : SupplementalController
    {
        private readonly ILogger<AuthController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;
        private readonly IApplogService appLogService;
        private string classFileName = "AuthController";

        public AuthController(ILogger<AuthController> logger, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration, 
            IEmailService emailService,
            IApplogService applogService) : base(userManager)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailService = emailService;
            this.appLogService = applogService;
        }

        [HttpGet]
        [Authorize]
        [Route("GetCurrentUser")]
        public async Task<ActionResult<ApplicationUserViewModel>> GetCurrentUser()
        {
            ApplicationUserViewModel applicationUser = new();
            this.logger.LogInformation("Getting Current User");
            try
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

                        applicationUser = this.mapper.Map<ApplicationUserViewModel>(user);
                        applicationUser.Roles = roles;
                    }
                }

                return this.Ok(applicationUser);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Something Went Wrong in the {nameof(this.GetCurrentUser)}");
                return this.Problem($"Something Went Wrong in the {nameof(this.GetCurrentUser)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(userDto.Email))
            {
                throw new InvalidDataException("User email or password empty");
            }

            try
            {
                this.logger.LogInformation($"Registration Attempt for {userDto.Email}");
                var user = this.mapper.Map<ApplicationUser>(userDto);
                user.UserName = userDto.Email;
                user.CreatedDate = DateTime.UtcNow;
                var result = await this.userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(error.Code, error.Description);
                    }

                    return this.BadRequest(this.ModelState);
                }

                await this.userManager.AddToRoleAsync(user, "User");

                string emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(emailToken);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

                await emailService.SendEmailVerificationMessageAsync(user, codeEncoded);

                return this.Accepted();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Something Went Wrong in the {nameof(this.Register)}");
                return this.Problem($"Something Went Wrong in the {nameof(this.Register)}", statusCode: 500);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("confirmemail")]
        public async Task<ActionResult<ApplicationUserViewModel>> ConfirmEmail([FromQuery]string token,[FromQuery] string email)
        {
            try
            {

                var user = await userManager.FindByEmailAsync(email);

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email) || user == null)
                {
                    AppLog log = new() { FileName = classFileName, Method = "ConfirmEmail", Project = Lookups.Project.Server, Message = $"Email verification url invalid: {email}", Severity = Lookups.Severity.Error };
                    await appLogService.UploadLogEntry(log, await GetApplicationUserId());

                    return BadRequest("Email verification url invalid.");
                }

                var codeDecodedBytes = WebEncoders.Base64UrlDecode(token);
                var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

                var result = await userManager.ConfirmEmailAsync(user, codeDecoded);

                if(result.Succeeded)
                {
                    return LocalRedirect("/users/emailconfirmed");
                }
                else
                {
                    return LocalRedirect("/EmailNotConfirmed");
                }
                
               
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Something Went Wrong in the {nameof(this.ConfirmEmail)}");
                return this.Problem($"Something Went Wrong in the {nameof(this.ConfirmEmail)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginUserDto userDto)
        {
            ApplicationUser? user = new();

            try
            {
                if (string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
                {
                    throw new InvalidDataException("Email or password null or empty");
                }

                this.logger.LogInformation($"Login Attempt for {userDto.Email}");

                user = await this.userManager.FindByEmailAsync(userDto.Email);

                if (user == null)
                {
                    throw new InvalidDataException("Email or password invalid");
                }

                var passwordValid = await this.userManager.CheckPasswordAsync(user, userDto.Password!);

                if (user == null || !passwordValid)
                {
                    return this.Unauthorized(userDto);
                }

                string tokenString = await this.GenerateToken(user);

                var response = new AuthResponse
                {
                    Email = userDto.Email!,
                    Token = tokenString,
                    UserId = user.Id,
                };

                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Something Went Wrong in the {nameof(this.Login)}: {ex.Message}");
                return this.Problem($"Something Went Wrong in the {nameof(this.Login)}: {ex.Message}", statusCode: 500);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("updateloggedinuser")]
        [Authorize]
        public async Task<ActionResult<ApplicationUserViewModel>> PutUser([FromBody] ApplicationUserViewModel applicationUserViewModel)
        {
            if (applicationUserViewModel == null)
            {
                return this.BadRequest();
            }

            if (applicationUserViewModel.Id == null)
            {
                return this.BadRequest();
            }

            ApplicationUser? applicationUser = await this.userManager.FindByIdAsync(applicationUserViewModel.Id);

            if (applicationUser == null)
            {
                return this.NotFound();
            }

            this.mapper.Map(applicationUserViewModel, applicationUser);

            try
            {
                await this.userManager.UpdateAsync(applicationUser);
                return this.Ok(applicationUserViewModel);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                this.logger.LogError(ex, $"Something Went Wrong in the {nameof(this.PutUser)}");
                return this.Problem($"Something Went Wrong in the {nameof(this.PutUser)}", statusCode: 500);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("updatepassword")]
        [Authorize]
        public async Task<ActionResult<ApplicationUserViewModel>> UpdatePassword([FromBody] UserProfileViewModel userProfileViewModel)
        {
            if (userProfileViewModel == null)
            {
                return this.BadRequest();
            }

            if (
                    userProfileViewModel.Email == null
                || userProfileViewModel.CurrentPassword == null
                || userProfileViewModel.Password == null
                || userProfileViewModel.ConfirmPassword == null
                || userProfileViewModel.Password != userProfileViewModel.ConfirmPassword
                || userProfileViewModel.Password == userProfileViewModel.CurrentPassword
                || userProfileViewModel.ConfirmPassword == userProfileViewModel.CurrentPassword)
            {
                return this.BadRequest();
            }

            ApplicationUser? applicationUser = await this.userManager.FindByEmailAsync(userProfileViewModel.Email);

            if (applicationUser == null)
            {
                return this.NotFound();
            }

            try
            {
                applicationUser!.ModifiedDate = DateTime.UtcNow;
                var result = await this.userManager.ChangePasswordAsync(applicationUser, userProfileViewModel.CurrentPassword, userProfileViewModel.Password);

                if (result.Succeeded)
                {
                    applicationUser = await this.userManager.FindByEmailAsync(userProfileViewModel.Email);

                    ApplicationUserViewModel applicationUserViewModel = this.mapper.Map<ApplicationUserViewModel>(applicationUser);

                    return this.Ok(applicationUserViewModel);
                }
                else
                {
                    return this.BadRequest(result);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                this.logger.LogError(ex, $"Something Went Wrong in the {nameof(this.UpdatePassword)}");
                return this.Problem($"Something Went Wrong in the {nameof(this.UpdatePassword)}", statusCode: 500);
            }
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var roles = await this.userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var userClaims = await this.userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(CustomClaimTypes.Uid, user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: this.configuration["JwtSettings:Issuer"],
                audience: this.configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(this.configuration["JwtSettings:Duration"])),
                signingCredentials: credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
#if DEBUG
            Debug.WriteLine($"token: {tokenString}");

#endif
            return tokenString;
        }
    }
}
