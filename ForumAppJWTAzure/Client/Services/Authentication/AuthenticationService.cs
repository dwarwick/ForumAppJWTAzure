namespace ForumAppJWTAzure.Client.Services.Authentication
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly HttpClient httpClient;

        public AuthenticationService(
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            HttpClient client)
            : base(client, localStorage)
        {
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
            this.httpClient = client;
        }

        public event EventHandler<ApplicationUserViewModel>? UpdatedUser;

        public event EventHandler<bool>? UserLoggedOut;

        public ApplicationUserViewModel? ApplicationUserViewModel { get; set; }

        public async Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel)
        {
            Response<AuthResponse> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(loginModel);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                var responseMessage = await this.httpClient.PostAsync(ApiEndpoints.Login, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<AuthResponse>(jsonString);

                    response = new Response<AuthResponse>
                    {
                        Data = myObject!,
                        Success = true,
                    };
                    ////Store Token
                    await this.localStorage.SetItemAsync("accessToken", response.Data?.Token ?? string.Empty);

                    ////Change auth state of app
                    await ((ApiAuthenticationStateProvider)this.authenticationStateProvider).LoggedIn();

                    Response<ApplicationUserViewModel> userResponse = await this.GetLoggedInUser();

                    if (userResponse.Success && userResponse.Data != null)
                    {
                        this.ApplicationUserViewModel = userResponse.Data;
                        this.UpdatedUser?.Invoke(this, this.ApplicationUserViewModel);
                    }

                    return response;
                }
                else
                {
                    return new Response<AuthResponse> { Data = new AuthResponse { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<AuthResponse>(exception);
            }

            return response;
        }

        public async Task<Response<ApplicationUserViewModel>> UpdateUserAsync(ApplicationUserViewModel applicationUser)
        {
            Response<ApplicationUserViewModel> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(applicationUser);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                var responseMessage = await this.httpClient.PutAsync(ApiEndpoints.UpdateLoggedInUser, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<ApplicationUserViewModel>(jsonString);

                    response = new Response<ApplicationUserViewModel>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    this.ApplicationUserViewModel = response.Data;
                    this.UpdatedUser?.Invoke(this, this.ApplicationUserViewModel);

                    return response;
                }
                else
                {
                    return new Response<ApplicationUserViewModel> { Data = new ApplicationUserViewModel { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<ApplicationUserViewModel>(exception);
            }

            return response;
        }

        public async Task<Response<ApplicationUserViewModel>> UpdatePasswordAsync(UserProfileViewModel userProfileViewModel)
        {
            Response<ApplicationUserViewModel> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(userProfileViewModel);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                var responseMessage = await this.httpClient.PutAsync(ApiEndpoints.UpdatePassword, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<ApplicationUserViewModel>(jsonString);

                    response = new Response<ApplicationUserViewModel>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    this.ApplicationUserViewModel = response.Data;
                    this.UpdatedUser?.Invoke(this, this.ApplicationUserViewModel);

                    return response;
                }
                else
                {
                    return new Response<ApplicationUserViewModel> { Data = new ApplicationUserViewModel { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<ApplicationUserViewModel>(exception);
            }

            return response;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)this.authenticationStateProvider).LoggedOut();

            this.ApplicationUserViewModel = null;
            this.UserLoggedOut?.Invoke(this, true);
        }

        public async Task<HttpResponseMessage> RegisterAsync(UserDto userDto)
        {
            var user = System.Text.Json.JsonSerializer.Serialize(userDto);
            var requestContent = new StringContent(user, Encoding.UTF8, "application/json");
            var responseMessage = await this.httpClient.PostAsync(ApiEndpoints.Register, requestContent);

            await this.localStorage.RemoveItemAsync("accessToken");

            return responseMessage;
        }

        public async Task<Response<ApplicationUserViewModel>> GetLoggedInUser()
        {
            Response<ApplicationUserViewModel> response = new();
            try
            {
                if (!await this.IsIauthenticated())
                {
                    return new Response<ApplicationUserViewModel>() { Success = false };
                }

                if (this.ApplicationUserViewModel != null)
                {
                    return new Response<ApplicationUserViewModel> { Success = true, Data = this.ApplicationUserViewModel };
                }

                var responseMessage = await this.httpClient.GetAsync(ApiEndpoints.GetLoggedInUser);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<ApplicationUserViewModel>(jsonString);

                    response = new Response<ApplicationUserViewModel>()
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    this.ApplicationUserViewModel = response.Data;

                    return response!;
                }
                else
                {
                    await Logout();
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<ApplicationUserViewModel>(exception);
            }

            return response;
        }

        public async Task<bool> IsIauthenticated()
        {
            bool result = await this.GetBearerToken();

            if (!result)
            {
                await this.Logout();
            }

            return result;
        }
    }
}
