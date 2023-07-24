namespace ForumAppJWTAzure.Client.Services
{
    public class SearchService : BaseHttpService, ISearchService
    {
        private readonly HttpClient client;

        public SearchService(HttpClient client, ILocalStorageService localStorage)
            : base(client, localStorage)
        {
            this.client = client;
        }

        public event EventHandler<string> SearchCompleted;

        public override Task<Response<T>> Create<T>(T model, string endPoint)
        {
            return base.Create(model, endPoint);
        }

        public override Task<Response<List<T>>> Get<T>(string endPoint)
        {
            return base.Get<T>(endPoint);
        }

        public async Task<Response<List<ForumViewModel>>> SearchBySearchTerm(SearchViewModel model)
        {
            Response<List<ForumViewModel>> response;

            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                var responseMessage = await this.client.PostAsync(ApiEndpoints.SearchBySearchTerm, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<List<ForumViewModel>>(jsonString);

                    response = new Response<List<ForumViewModel>>
                    {
                        Data = myObject ?? new List<ForumViewModel>(),
                        Success = true,
                    };

                    this.SearchCompleted.Invoke(this, model.SearchTerm ?? string.Empty);
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
    }
}
