namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ForumController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // GET: api/Forum
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ForumViewModel>>> GetForums()
        {
            List<Forum>? forums = null;

            if (this.context.Forums == null)
            {
                return this.NotFound();
            }

            try
            {
                forums = await this.context.Forums.OrderByDescending(x => x.CreatedDate).Include(x => x.CreatedBy)
                            .Include(y => y.Posts).ThenInclude(x => x.Votes)
                            .Include(y => y.Posts.OrderByDescending(x => x.CreatedDate)).ThenInclude(x => x.CreatedBy)
                            .Include(y => y.Tags).ThenInclude(x => x.CreatedBy)
                            .Include(y => y.Followers)
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to get forum list.\n\n{ex}");
            }

            return this.Ok(this.mapper.Map<List<ForumViewModel>>(forums));
        }

        // GET: api/Forum/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ForumViewModel>> GetForumViewModel(int id)
        {
            if (this.context.Forums == null)
            {
                return this.NotFound();
            }

            var forum = await this.context.Forums.FindAsync(id);

            if (forum == null)
            {
                return this.NotFound();
            }

            return this.Ok(forum);
        }

        // PUT: api/Forum/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutForumViewModel(int id, ForumViewModel forumViewModel)
        {
            if (id != forumViewModel.Id)
            {
                return this.BadRequest();
            }

            this.context.Entry(forumViewModel).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ForumViewModelExists(id))
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

        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("createnewforum")]
        public async Task<ActionResult<ForumViewModel>> PostForum(ForumViewModel forumViewModel)
        {
            Forum? forum = null;

            try
            {
                var viewModelTags = forumViewModel.Tags;

                List<Tag> tags = new List<Tag>();

                foreach (var item in viewModelTags)
                {
                    Tag? tag = await this.context.Tags.FirstOrDefaultAsync(x => x.Name == item.Name);
                    if (tag != null)
                    {
                        tags.Add(tag);
                    }
                }

                forum = this.mapper.Map<Forum>(forumViewModel);
                forum.Tags = new();
                forum.Tags.AddRange(tags);

                this.context.Add(forum);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to post new forum.\n\n{ex}");
            }

            forumViewModel.Id = forum.Id;

            return this.CreatedAtAction("GetForumViewModel", new { id = forumViewModel.Id }, forumViewModel);
        }

        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("followforum")]
        public async Task<ActionResult<ForumViewModel>> FollowForum(FollowedForumViewModel model)
        {
            try
            {
                if (!context.FollowedForums.Any(x => x.FollowerId == model.FollowerId && x.ForumId == model.ForumId))
                {
                    FollowedForum mapped = mapper.Map<FollowedForum>(model);
                    context.FollowedForums.Add(mapped);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return this.Problem($"User is already following forum.");
                }
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to follow forum.\n\n{ex}");
            }



            return this.CreatedAtAction("FollowForum", new { id = model.ForumId }, model);
        }

        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete]
        [Route("{forumId}/unfollowforum/{userId}")]
        public async Task<ActionResult<ForumViewModel>> UnFollowForum([FromRoute] int forumId, [FromRoute] string userId)
        {
            try
            {
                if (context.FollowedForums.Any(x => x.FollowerId == userId && x.ForumId == forumId))
                {
                    FollowedForum followedForum = new()
                    {
                        FollowerId = userId, 
                        ForumId = forumId
                    };

                    context.FollowedForums.Remove(followedForum);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return this.Problem($"User is not following forum.");
                }
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to un-follow forum.\n\n{ex}");
            }



            return this.CreatedAtAction("UnFollowForum", new { id = forumId });
        }

        // DELETE: api/Forum/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForumViewModel(int id)
        {
            if (this.context.Forums == null)
            {
                return this.NotFound();
            }

            var forumViewModel = await this.context.Forums.FindAsync(id);
            if (forumViewModel == null)
            {
                return this.NotFound();
            }

            this.context.Forums.Remove(forumViewModel);
            await this.context.SaveChangesAsync();

            return this.NoContent();
        }

        private bool ForumViewModelExists(int id)
        {
            return (this.context.Forums?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
