using Microsoft.AspNetCore.SignalR;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PostController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // POST: api/Forum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("createnewpost")]
        public async Task<ActionResult<PostViewModel>> PostPostViewModel(PostViewModel postViewModel)
        {
            if (this.context.Forums == null)
            {
                return this.Problem("Entity set 'BookStoreDbContext.ForumViewModel'  is null.");
            }

            Post? post = null;

            try
            {
                post = this.mapper.Map<Post>(postViewModel);
                this.context.Posts.Add(post);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to post new post.\n\n{ex}");
            }

            postViewModel.Id = post.Id;

            return this.CreatedAtAction("PostPostViewModel", new { id = postViewModel.Id }, postViewModel);
        }

        [HttpPut]
        [Route("editpost")]
        public async Task<ActionResult<PostViewModel>> EditPostViewModel(PostViewModel postViewModel)
        {
            if (this.context.Forums == null)
            {
                return this.Problem("Entity set 'BookStoreDbContext.ForumViewModel'  is null.");
            }

            Post? post = null;

            try
            {
                post = this.mapper.Map<Post>(postViewModel);

                this.context.Posts.Attach(post);
                this.context.Entry(post).Property(x => x.Text).IsModified = true;
                this.context.Entry(post).Property(x => x.ModifiedDate).IsModified = true;
                this.context.Posts.Update(post);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return this.Problem($"Unable to post new post.\n\n{ex}");
            }

            postViewModel.Id = post.Id;

            return this.CreatedAtAction("PostPostViewModel", new { id = postViewModel.Id }, postViewModel);
        }
    }
}
