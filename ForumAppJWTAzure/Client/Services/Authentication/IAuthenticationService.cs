namespace ForumAppJWTAzure.Client.Services.Authentication
{
    public interface IAuthenticationService
    {
        event EventHandler<ApplicationUserViewModel>? UpdatedUser;

        event EventHandler<bool>? UserLoggedOut;

        Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel);

        Task<HttpResponseMessage> RegisterAsync(UserDto userDto);

        Task<Response<ApplicationUserViewModel>> GetLoggedInUser();

        Task<Response<ApplicationUserViewModel>> UpdateUserAsync(ApplicationUserViewModel applicationUser);

        Task<Response<ApplicationUserViewModel>> UpdatePasswordAsync(UserProfileViewModel userProfileViewModel);

        Task Logout();

        Task<bool> IsIauthenticated();
    }
}
