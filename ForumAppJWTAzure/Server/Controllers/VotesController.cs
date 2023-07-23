namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public VotesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Votes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
            if (this.context.Votes == null)
            {
                return this.NotFound();
            }

            return await this.context.Votes.ToListAsync();
        }

        // GET: api/Votes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(int id)
        {
            if (this.context.Votes == null)
            {
                return this.NotFound();
            }

            var vote = await this.context.Votes.FindAsync(id);

            if (vote == null)
            {
                return this.NotFound();
            }

            return vote;
        }

        // PUT: api/Votes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote(int id, Vote vote)
        {
            if (id != vote.Id)
            {
                return this.BadRequest();
            }

            this.context.Entry(vote).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.VoteExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/Votes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VoteViewModel>> PostVote(VoteViewModel voteViewModel)
        {
            if (voteViewModel == null)
            {
                return this.Problem("voteViewModel  is null.");
            }

            Vote vote = this.mapper.Map<Vote>(voteViewModel);

            var existingVote = await this.context.Votes.FirstOrDefaultAsync(x => x.PostId == vote.PostId && x.CreatedById == vote.CreatedById);

            if (existingVote != null)
            {
                this.context.Votes.Remove(existingVote);
                await this.context.SaveChangesAsync();
            }

            this.context.Votes.Add(vote);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("PostVote", this.mapper.Map<VoteViewModel>(vote));
        }

        // DELETE: api/Votes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            if (this.context.Votes == null)
            {
                return this.NotFound();
            }

            var vote = await this.context.Votes.FindAsync(id);
            if (vote == null)
            {
                return this.NotFound();
            }

            this.context.Votes.Remove(vote);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool VoteExists(int id)
        {
            return (this.context.Votes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
