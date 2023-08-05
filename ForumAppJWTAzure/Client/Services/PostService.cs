
using ForumAppJWTAzure.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
namespace ForumAppJWTAzure.Client.Services
{
    public class PostService : BaseHttpService, IPostService
    {
        private readonly HttpClient client;

        private readonly ISignalRService? hubConnection;

        public PostService(HttpClient client, ILocalStorageService localStorage, ISignalRService hubConnection)
            : base(client, localStorage)
        {
            this.client = client;
            this.hubConnection = hubConnection;
        }

        public async Task<Response<PostViewModel>> CreateNewPost(PostViewModel model)
        {
            Response<PostViewModel> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return new Response<PostViewModel>() { Success = false, Message = "Not authorized" };
                }

                var responseMessage = await this.client.PostAsync(ApiEndpoints.CreateNewPost, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<PostViewModel>(jsonString);

                    response = new Response<PostViewModel>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    if (this.hubConnection != null)
                    {
                        string serializedPost = JsonConvert.SerializeObject(response.Data, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                        if (response.Data?.ForumId != null && this.hubConnection != null && this.hubConnection.HubConnection != null)
                        {
                            await this.hubConnection.HubConnection.SendAsync("AddPostViewModel", serializedPost ?? string.Empty, response.Data.ForumId ?? 0);
                        }
                    }

                    return response;
                }
                else
                {
                    return new Response<PostViewModel> { Data = new PostViewModel { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<PostViewModel>(exception);
            }

            return response;
        }

        public async Task<Response<PostViewModel>> EditPost(PostViewModel model)
        {
            Response<PostViewModel> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return new Response<PostViewModel>() { Success = false, Message = "Not authorized" };
                }

                var responseMessage = await this.client.PutAsync(ApiEndpoints.EditPost, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<PostViewModel>(jsonString);

                    response = new Response<PostViewModel>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    if (this.hubConnection != null)
                    {
                        string serializedPost = JsonConvert.SerializeObject(response.Data, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                        if (response.Data?.ForumId != null && this.hubConnection != null && this.hubConnection.HubConnection != null)
                        {
                            await this.hubConnection.HubConnection.SendAsync("AddPostViewModel", serializedPost ?? string.Empty, response.Data.ForumId ?? 0);
                        }
                    }

                    return response;
                }
                else
                {
                    return new Response<PostViewModel> { Data = new PostViewModel { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<PostViewModel>(exception);
            }

            return response;
        }

        public async Task<Response<LocationViewModel>> UploadImage(UploadFileModel model)
        {
            Response<LocationViewModel> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return new Response<LocationViewModel>() { Success = false, Message = "Not authorized" };
                }

                

                

                var responseMessage = await this.client.PostAsync(ApiEndpoints.UploadPostPic, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<LocationViewModel>(jsonString);

                    response = new Response<LocationViewModel>
                    {
                        Data = myObject!,
                        Success = true,
                    };                    

                    return response;
                }
                else
                {
                    return new Response<LocationViewModel> { Data = new LocationViewModel { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<LocationViewModel>(exception);
            }

            return response;
        }
    }
}
