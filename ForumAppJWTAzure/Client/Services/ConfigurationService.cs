using System.Runtime.CompilerServices;

namespace ForumAppJWTAzure.Client.Services
{
    public class ConfigurationService : BaseHttpService, IConfigurationService
    {
        public Dictionary<string, string> Configuration { get; set; }

        private readonly HttpClient client;

        public ConfigurationService(HttpClient client, ILocalStorageService localStorage) : base(client, localStorage)
        {
            this.client = client;

            
        }

        public async Task GetSettings()
        {
            Configuration = new Dictionary<string, string>();
            string value = string.Empty;

            value = await Get("BlobStorage:post-pic-container", ApiEndpoints.Configuration);
            Configuration.Add("BlobStorage:post-pic-container", value);

            value = await Get("BlobStorage:profile-pic-container", ApiEndpoints.Configuration);
            Configuration.Add("BlobStorage:profile-pic-container", value);

            value = await Get("SignalRHubName", ApiEndpoints.Configuration);
            Configuration.Add("SignalRHubName", value);

            value = await Get("ApiUrl", ApiEndpoints.Configuration);
            Configuration.Add("ApiUrl", value);
        }

        public async Task<string> Get(string key,  string endPoint)
        {
            string test = "";
            //try
            //{
            try
            {
                test = await this.client.GetStringAsync($"{endPoint}/{key}");
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }

            return test;
                //if (responseMessage.IsSuccessStatusCode)
                //{
                //    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                //    var myObject = JsonConvert.DeserializeObject<string>(jsonString);

                //    response = new Response<string>
                //    {
                //        Data = myObject ?? string.Empty,
                //        Success = true,
                //    };

                //    return response;
                //}
                //else
                //{
                //    return new Response<string> { Data =string.Empty, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                //}
            //}
            //catch (ApiException exception)
            //{
            //    response = this.ConvertApiExceptions<string>(exception);
            //}

            //return response;
        }
    }
}
