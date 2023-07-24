namespace ForumAppJWTAzure.Client.Services.Base
{
    public partial class ApiException : System.Exception
    {
        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response[..((response?.Length ?? 0) >= 512 ? 512 : (response?.Length ?? 0))]), innerException)
        {
            this.StatusCode = statusCode;
            this.Response = response ?? string.Empty;
            this.Headers = headers;
        }

        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", this.Response, base.ToString());
        }
    }

    public partial class ApiException<TResult> : ApiException
    {
        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            this.Result = result;
        }

        public TResult Result { get; private set; }
    }
}
