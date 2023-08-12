namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public TagsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> GetTags()
        {
            if (this.context.Tags == null)
            {
                return this.NotFound();
            }

            return await this.context.Tags.OrderBy(x => x.Name).ToListAsync();
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            if (this.context.Tags == null)
            {
                return this.NotFound();
            }

            var tag = await this.context.Tags.FindAsync(id);

            if (tag == null)
            {
                return this.NotFound();
            }

            return tag;
        }

        // PUT: api/Tags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return this.BadRequest();
            }

            this.context.Entry(tag).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.TagExists(id))
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

        // POST: api/Tags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(TagViewModel tagViewModel)
        {
            if (this.context.Tags == null)
            {
                return this.Problem("Entity set 'ApplicationDbContext.Tags'  is null.");
            }

            Tag tag = this.mapper.Map<Tag>(tagViewModel);
            this.context.Tags.Add(tag);
            await this.context.SaveChangesAsync();

            return this.CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }
        
        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            if (this.context.Tags == null)
            {
                return this.NotFound();
            }

            var tag = await this.context.Tags.FindAsync(id);
            if (tag == null)
            {
                return this.NotFound();
            }

            this.context.Tags.Remove(tag);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool TagExists(int id)
        {
            return (this.context.Tags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
