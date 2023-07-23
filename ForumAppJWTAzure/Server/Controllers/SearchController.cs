namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearch search;
        private readonly IConfiguration configuration;

        public SearchController(ISearch search, IConfiguration configuration)
        {
            this.search = search;
            this.configuration = configuration;
        }

        // GET: api/<SearchController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SearchController>/5
        [HttpPost("searchbysearchterm")]
        public async Task<ActionResult<List<ForumViewModel>>> SearchBySearchTerm([FromBody] SearchViewModel model)
        {
            string searchTerm = model.SearchTerm ?? string.Empty;
            ISearchResponse<ForumViewModel>? response;
            if (this.search.Client != null)
            {
                response = await this.search.Client.SearchAsync<ForumViewModel>(s => s
                    .Query(q => q
                        .Bool(b => b
                            .Should(
                                sh => sh
                                .Match(c => c
                                    .Field(p => p.Title).Analyzer("default_search")
                                    .Query(searchTerm)),
                                sh => sh
                                .Match(c => c
                                    .Field(p => p.Posts[0].Text).Analyzer("default_search")
                                    .Query(searchTerm)),
                                sh => sh
                                .Match(c => c
                                    .Field(p => p.Tags[0].Name).Analyzer("default_search")
                                    .Query(searchTerm))))));
                return this.Ok(response.Documents.ToList());
            }

            return this.BadRequest();
        }

        // POST api/<SearchController>
        [HttpPost]
        public async Task<ActionResult<ForumViewModel>> Post([FromBody] ForumViewModel model)
        {
            string indexName = this.configuration["Elastic:SearchIndex"] ?? string.Empty;

            this.search.CreateIndex(indexName);

            IndexResponse response;
            if (model != null && this.search.Client != null)
            {
                response = await this.search.Client.IndexDocumentAsync<ForumViewModel>(model);

                if (response.IsValid)
                {
                    Console.WriteLine($"Index document with ID {response.Id} succeeded.");
                    return this.Ok(model);
                }
            }

            return this.BadRequest();
        }
    }
}