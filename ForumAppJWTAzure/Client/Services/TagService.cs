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
    }
}
