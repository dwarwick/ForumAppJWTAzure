using Blazored.LocalStorage;
using System.Net;
using System.Text;

namespace ForumAppJWTAzure.Client.Services.Base
{
    public class BaseHttpService
    {
        private readonly HttpClient client;
        private readonly ILocalStorageService localStorage;

        public BaseHttpService(HttpClient client, ILocalStorageService localStorage)
        {
            this.client = client;
            this.localStorage = localStorage;
        }

       

        public virtual async Task<Response<T>> Create<T>(T model, string endPoint)
        {
            Response<T> response;
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return new Response<T>() { Success = false, Message = "Not authorized" };
                }

                var responseMessage = await this.client.PostAsync(endPoint, requestContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<T>(jsonString);

                    response = new Response<T>
                    {
                        Data = myObject!,
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<T> { Data = (T)Activator.CreateInstance(typeof(T))!, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<T>(exception);
            }

            return response;
        }

        public virtual async Task<Response<List<T>>> Get<T>(string endPoint)
        {
            Response<List<T>> response;
            try
            {
                var responseMessage = await this.client.GetAsync(endPoint);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<List<T>>(jsonString);

                    response = new Response<List<T>>
                    {
                        Data = myObject ?? new List<T>(),
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<List<T>> { Data = new List<T> { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<List<T>>(exception);
            }

            return response;
        }

        public virtual async Task<Response<T>> GetSingle<T>(string endPoint)
        {
            Response<T> response;
            try
            {
                var responseMessage = await this.client.GetAsync(endPoint);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<T>(jsonString);

                    response = new Response<T>
                    {
                        Data = myObject,
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<T> { Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<T>(exception);
            }

            return response;
        }

        public virtual async Task<Response<List<T>>> GetAll<T>(string endPoint)
        {
            Response<List<T>> response;
            try
            {
                var responseMessage = await this.client.GetAsync(endPoint);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = responseMessage.Content.ReadAsStringAsync().Result;
                    var myObject = JsonConvert.DeserializeObject<List<T>>(jsonString);

                    response = new Response<List<T>>
                    {
                        Data = myObject ?? new List<T>(),
                        Success = true,
                    };

                    return response;
                }
                else
                {
                    return new Response<List<T>> { Data = new List<T> { }, Message = responseMessage.ReasonPhrase ?? string.Empty, Success = false };
                }
            }
            catch (ApiException exception)
            {
                response = this.ConvertApiExceptions<List<T>>(exception);
            }

            return response;
        }

        public virtual async Task<bool> Put<T>(T model, string endPoint)
        {            
            try
            {
                var user = System.Text.Json.JsonSerializer.Serialize(model);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");

                if (!await this.GetBearerToken())
                {
                    return false;
                }

                var responseMessage = await this.client.PutAsync(endPoint, requestContent);

                return responseMessage.StatusCode == HttpStatusCode.NoContent;
                
            }
            catch (ApiException)
            {
                
            }

            return false;
        }

        public virtual async Task<bool> DeleteAsync<T>(T model, string endPoint)
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

                response = await this.client.DeleteAsync(endPoint);

                return response.IsSuccessStatusCode;
            }
            catch (ApiException exception)
            {
                Console.WriteLine(exception.Message);
            }

            return false;
        }

        protected Response<TGuid> ConvertApiExceptions<TGuid>(ApiException apiException)
        {
            if (apiException.StatusCode == 400)
            {
                return new Response<TGuid>() { Message = "Validation errors have occured.", ValidationErrors = apiException.Response, Success = false };
            }

            if (apiException.StatusCode == 404)
            {
                return new Response<TGuid>() { Message = "The requested item could not be found.", Success = false };
            }

            if (apiException.StatusCode == 401)
            {
                return new Response<TGuid>() { Message = "Invalid Credentials, Please Try Again", Success = false };
            }

            if (apiException.StatusCode >= 200 && apiException.StatusCode <= 299)
            {
                return new Response<TGuid>() { Message = "Operation Reported Success", Success = true };
            }

            return new Response<TGuid>() { Message = "Something went wrong, please try again.", Success = false };
        }

        protected async Task<bool> GetBearerToken()
        {
            var token = await this.localStorage.GetItemAsync<string>("accessToken");

            if (string.IsNullOrEmpty(token))
            {
                this.client.DefaultRequestHeaders.Authorization = null;

                return false;
            }

            if (CheckTokenIsValid(token))
            {
                this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            else
            {
                await this.localStorage.RemoveItemAsync("accessToken");
            }

            return false;
        }

        public static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }

        public static bool CheckTokenIsValid(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }
    }
}
