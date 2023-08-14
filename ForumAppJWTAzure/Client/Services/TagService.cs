using ForumAppJWTAzure.Shared.Models;

namespace ForumAppJWTAzure.Client.Services
{
    public class TagService : BaseHttpService, ITagService
    {
        private readonly HttpClient client;

        public TagService(HttpClient client, ILocalStorageService localStorage)
            : base(client, localStorage)
        {
            this.client = client;
        }

        public override Task<Response<T>> Create<T>(T model, string endPoint)
        {
            return base.Create(model, endPoint);
        }

        public override Task<Response<List<T>>> Get<T>(string endPoint)
        {
            return base.Get<T>(endPoint);
        }

        public async Task<Response<List<TagViewModel>>> GetAllTags()
        {
            Response<List<TagViewModel>> response;
            try
            {
                var responseMessage = await this.client.GetAsync(ApiEndpoints.GetAllTags);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<List<TagViewModel>>(jsonString);

                    response = new Response<List<TagViewModel>>
                    {
                        Data = myObject ?? new List<TagViewModel>(),
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<List<TagViewModel>> { Data = new List<TagViewModel> { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<List<TagViewModel>>(exception);
            }

            return response;
        }

        public async Task<Response<List<TagViewModel>>> GetSuggestedTags(ForumViewModel forum)
        {
            Response<List<TagViewModel>> response;

            List<ModelInput> inputs = new()
            {
                new()
                {
                    Title = forum?.Title ?? string.Empty,
                    Body = forum ?.PostText ?? string.Empty,
                    Tags = "",
                }
            };

            inputs = HtmlHelpers.RemovePreNodes(inputs);

            ModelInput model = inputs.FirstOrDefault() ?? new() { Body = string.Empty, Tags = string.Empty, Title = string.Empty};

            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return new Response<List<TagViewModel>>() { Success = false, Message = "Not authorized" };
                }

                var responseMessage = await this.client.PostAsync(ApiEndpoints.PredictTags, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<List<TagViewModel>>(jsonString);

                    response = new Response<List<TagViewModel>>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<List<TagViewModel>> { Data = (List<TagViewModel>)Activator.CreateInstance(typeof(List<TagViewModel>))!, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<List<TagViewModel>>(exception);
            }

            return response;
        }
    }
}
