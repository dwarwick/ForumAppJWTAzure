namespace ForumAppJWTAzure.Client.Services
{
    public class StorageService : BaseHttpService
    {
        private readonly HttpClient client;

        public StorageService(HttpClient client, ILocalStorageService localStorage)
            : base(client, localStorage)
        {
            this.client = client;
        }

        public async Task<Response<StorageViewModel>> UploadAsync3(StorageViewModel model)
        {
            Response<StorageViewModel> response;
            try
            {
                var payload = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return new Response<StorageViewModel>() { Success = false, Message = "Not authorized" };
                }

                var responseMessage = await this.client.PostAsync(ApiEndpoints.UploadImageToStorage, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<StorageViewModel>(jsonString);

                    response = new Response<StorageViewModel>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<StorageViewModel> { Data = new StorageViewModel { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<StorageViewModel>(exception);
            }

            return response;
        }

        public async Task<bool> DeleteAsync(StorageViewModel model)
        {
            HttpResponseMessage response;
            try
            {
                var payload = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return false;
                }

                response = await this.client.PostAsync(ApiEndpoints.DeleteProfilePic, requestContent);

                return response.IsSuccessStatusCode;
            }
            catch (ApiException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return false;
        }
    }
}
