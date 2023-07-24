namespace ForumAppJWTAzure.Client.Services
{
    public class ForumService : BaseHttpService, IForumService
    {
        private readonly HttpClient client;

        public ForumService(HttpClient client, ILocalStorageService localStorage)
            : base(client, localStorage)
        {
            this.client = client;
        }

        public async Task<Response<List<ForumViewModel>>> GetAllForums()
        {
            Response<List<ForumViewModel>> response;
            try
            {
                var responseMessage = await this.client.GetAsync(ApiEndpoints.GetAllForums);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<List<ForumViewModel>>(jsonString);

                    response = new Response<List<ForumViewModel>>
                    {
                        Data = myObject ?? new List<ForumViewModel>(),
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<List<ForumViewModel>> { Data = new List<ForumViewModel> { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<List<ForumViewModel>>(exception);
            }

            return response;
        }

        public async Task<Response<ForumViewModel>> CreateNewForum(ForumViewModel model)
        {
            Response<ForumViewModel> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return new Response<ForumViewModel>() { Success = false, Message = "Not authorized" };
                }

                var responseMessage = await this.client.PostAsync(ApiEndpoints.CreateNewForum, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<ForumViewModel>(jsonString);

                    response = new Response<ForumViewModel>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<ForumViewModel> { Data = new ForumViewModel { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<ForumViewModel>(exception);
            }

            return response;
        }
    }
}
