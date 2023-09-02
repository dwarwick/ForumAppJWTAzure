using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumAppJWTAzure.Server.Data;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowedForumsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FollowedForumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FollowedForums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowedForum>>> GetFollowedPosts()
        {
          if (_context.FollowedForums == null)
          {
              return NotFound();
          }
            return await _context.FollowedForums.ToListAsync();
        }

        // GET: api/FollowedForums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FollowedForum>> GetFollowedForum(string id)
        {
          if (_context.FollowedForums == null)
          {
              return NotFound();
          }
            var followedForum = await _context.FollowedForums.FindAsync(id);

            if (followedForum == null)
            {
                return NotFound();
            }

            return followedForum;
        }

        // PUT: api/FollowedForums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFollowedForum(string id, FollowedForum followedForum)
        {
            if (id != followedForum.FollowerId)
            {
                return BadRequest();
            }

            _context.Entry(followedForum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowedForumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FollowedForums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FollowedForum>> PostFollowedForum(FollowedForum followedForum)
        {
          if (_context.FollowedForums == null)
          {
              return Problem("Entity set 'ApplicationDbContext.FollowedPosts'  is null.");
          }
            _context.FollowedForums.Add(followedForum);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FollowedForumExists(followedForum.FollowerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFollowedForum", new { id = followedForum.FollowerId }, followedForum);
        }

        // DELETE: api/FollowedForums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFollowedForum(string id)
        {
            if (_context.FollowedForums == null)
            {
                return NotFound();
            }
            var followedForum = await _context.FollowedForums.FindAsync(id);
            if (followedForum == null)
            {
                return NotFound();
            }

            _context.FollowedForums.Remove(followedForum);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FollowedForumExists(string id)
        {
            return (_context.FollowedForums?.Any(e => e.FollowerId == id)).GetValueOrDefault();
        }
    }
}
