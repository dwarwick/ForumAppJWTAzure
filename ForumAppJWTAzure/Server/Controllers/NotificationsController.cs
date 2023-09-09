using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumAppJWTAzure.Server.Data;
using ForumAppJWTAzure.Client.Services.Base;

namespace ForumAppJWTAzure.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public NotificationsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Notifications
        [HttpGet]
        [Route("{userId}/getnotifications")]
        public async Task<ActionResult<List<NotificationViewModel>>> Get([FromRoute] string userId)
        {
            Response<List<NotificationViewModel>>? response = new()
            {
                Success = false,
            };
            
            if (_context.Notifications == null)
            {
                return NotFound();
            }

            List<Notification> notifications = await _context.Notifications.Where(x => x.CreatedById == userId && x.Read == false).ToListAsync();
          
            if(notifications == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<List<NotificationViewModel>>(notifications));            
        }

        // GET: api/Notifications
        [HttpGet]
        [Route("getnotification/{Id}")]
        public async Task<ActionResult<NotificationViewModel>> GetNotification([FromRoute] int Id)
        {
            Response<NotificationViewModel>? response = new()
            {
                Success = false,
            };

            if (_context.Notifications == null)
            {
                return NotFound();
            }

            Notification? notification = await _context.Notifications.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (notification == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<NotificationViewModel>(notification));
        }

        // PUT: api/Notifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutNotification(NotificationViewModel notification)
        {
            if (notification == null)
            {
                return BadRequest();
            }

            Notification mapped = mapper.Map<Notification>(notification);

            _context.Entry(mapped).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Notifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notification)
        {
          if (_context.Notifications == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Notifications'  is null.");
          }
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostNotification", new { id = notification.Id }, notification);
        }

        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (_context.Notifications == null)
            {
                return NotFound();
            }
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationExists(int id)
        {
            return (_context.Notifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
