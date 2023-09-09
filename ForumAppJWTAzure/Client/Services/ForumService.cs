using Microsoft.AspNetCore.SignalR.Client;

namespace ForumAppJWTAzure.Client.Services
{
    public class ForumService : BaseHttpService, IForumService
    {
        private readonly HttpClient client;
        private readonly ISignalRService hubConnection;

        public ForumService(HttpClient client, ILocalStorageService localStorage, ISignalRService hubConnection)
            : base(client, localStorage)
        {
            this.client = client;
            this.hubConnection = hubConnection;
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

        public async Task<Response<ForumViewModel>> GetForum(int forumId)
        {
            Response<ForumViewModel> response;
            try
            {
                var responseMessage = await this.client.GetAsync(ApiEndpoints.GetForum(forumId));

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<ForumViewModel>(jsonString);

                    response = new Response<ForumViewModel>
                    {
                        Data = myObject ?? new ForumViewModel(),
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

        public async Task<Response<FollowedForumViewModel>> FollowAsync(FollowedForumViewModel model, string endPoint)
        {
            Response<FollowedForumViewModel> response = await base.Create(model, endPoint);

            if(response.Success)
            {
                if (this.hubConnection != null)
                {
                    string serializedFollowedForumViewModel = JsonConvert.SerializeObject(response.Data, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                    ForumChangedViewModel forumChangedViewModel = new()
                    {
                        ViewModel = serializedFollowedForumViewModel,
                        Message = $"You are now following {model.Title}"
                    };

                    if (this.hubConnection != null && this.hubConnection.HubConnection != null)
                    {
                        await this.hubConnection.HubConnection.SendAsync("ForumChanged", serializedFollowedForumViewModel ?? string.Empty, model.ForumId);
                    }
                }
            }

            return response;
        }

        public Task<bool> UnfollowAsync<T>(T model, string endPoint)
        {
            return base.DeleteAsync(model, endPoint);
        }
    }
}
