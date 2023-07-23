using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ForumAppJWTAzure.Server.Services
{
    public class Search : ISearch
    {
        private readonly IConfiguration configuration;

        public Search(IConfiguration configuration)
        {
            this.configuration = configuration;

            this.Start();
        }

        public ElasticClient? Client { get; set; }

        public void Start()
        {
            string fingerprint = this.configuration["Elastic:CertificateFingerprint"] ?? string.Empty;
            string username = this.configuration["Elastic:Username"] ?? string.Empty;
            string password = this.configuration["Elastic:Password"] ?? string.Empty;
            string url = this.configuration["Elastic:Url"] ?? string.Empty;
            string searchIndex = this.configuration["Elastic:SearchIndex"] ?? string.Empty;

            var settings = new ConnectionSettings(new Uri(url))
                //.CertificateFingerprint(fingerprint)
                .BasicAuthentication(username, password)
                .DefaultIndex(searchIndex)
                .EnableApiVersioningHeader();

            this.Client = new ElasticClient(settings);
        }

        public void CreateIndex(string indexName)
        {
            if (!string.IsNullOrEmpty(indexName) && !(this.Client?.Indices.Exists(indexName).Exists ?? false))
            {
                var createIndexResponse = this.Client.Indices.Create(indexName, c => c
                    .Settings(s => s
                        .Analysis(a =>
                            a.TokenFilters(tf => tf.Stop("english_stop", x => x.StopWords("_english_"))
                            .Stemmer("english_stemmer", x => x.Language("english")))
                            .Analyzers(x => x.Custom("default_search", y => y.Tokenizer("standard")
                            .Filters(new string[] { "lowercase", "english_stop", "english_stemmer" })))))
                    .Map<ForumViewModel>(m => m
                        .Properties(ps => ps
                            .Text(s => s.Name(n => n.Title))
                            .Text(s => s.Name(e => e.Posts[0].Text))
                            .Text(s => s.Name(e => e.Tags[0].Name)))));
            }
        }
    }
}
