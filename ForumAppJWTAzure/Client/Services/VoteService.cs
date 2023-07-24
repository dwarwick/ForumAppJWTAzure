namespace ForumAppJWTAzure.Client.Services
{
    public class VoteService : BaseHttpService, IVoteService
    {
        public VoteService(HttpClient client, ILocalStorageService localStorage)
            : base(client, localStorage)
        {
        }

        public override Task<Response<T>> Create<T>(T model, string endPoint)
        {
            return base.Create(model, endPoint);
        }

        public override Task<Response<List<T>>> Get<T>(string endPoint)
        {
            return base.Get<T>(endPoint);
        }
    }
}
