

using Nest;

namespace ForumAppJWTAzure.Server.Services
{
    public interface ISearch
    {
        public ElasticClient? Client { get; set; }

        public void CreateIndex(string indexName);
    }
}