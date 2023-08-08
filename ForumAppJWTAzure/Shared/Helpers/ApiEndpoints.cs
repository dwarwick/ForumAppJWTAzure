namespace ForumAppJWTAzure.Shared.Helpers
{
    public static class ApiEndpoints
    {
        public const string Login = "api/auth/login";
        public const string Logout = "api/auth/logout";
        public const string Register = "api/auth/register";
        public const string GetLoggedInUser = "api/auth/GetCurrentUser";
        public const string UpdateLoggedInUser = "api/auth/updateloggedinuser";
        public const string UpdatePassword = "api/auth/updatepassword";
        public static string ConfirmEmail(string url, string token, string email) => $"{url}/api/auth/confirmemail/?token={token}&email={email}";

        public const string CreateNewForum = "api/forum/createnewforum";
        public const string CreateNewPost = "api/post/createnewpost";
        public const string EditPost = "api/post/editpost";
        public const string GetAllForums = "api/forum";
        public const string GetAllTags = "api/tags";
        public const string CreateNewTag = "api/tags";
        public const string UploadToStorage = "api/storage";
        public const string UploadImageToStorage = "api/storage/uploadprofilepic";
        public const string UploadPostPic = "api/storage/uploadpostpic2";
        public const string DeleteProfilePic = "api/storage/deleteprofilepic";
        public const string Search = "api/search";
        public const string Vote = "api/votes";
        public const string AppLog = "api/applog";
        public const string Configuration = "api/appsetting";
        public const string ML = "api/ml";

        public const string SearchBySearchTerm = "api/search/searchbysearchterm";
    }
}
