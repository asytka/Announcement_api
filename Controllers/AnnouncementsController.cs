using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Announcement_api.Data;
using Announcement_api.Model;

namespace Announcement_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly Announcement_apiContext _context;

        public AnnouncementsController(Announcement_apiContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAnnouncement()
        {
            return await _context.Announcement.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Announcement>> GetAnnouncement([FromQuery] bool sims, int id)
        {

            bool AreSimilar(string text1, string text2)
            {
                var words1 = text1.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct();
                var words2 = text2.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct();

                return words1.Intersect(words2).Any();
            }

            var announcement = await _context.Announcement.FindAsync(id);

            if (announcement == null)
            {
                return NotFound();
            }

            if (!sims) { return announcement; }

            var announcements = await _context.Announcement.ToListAsync();

            var similar = announcements
                 .Where(a => a.id != announcement.id &&
                        AreSimilar($"{announcement.Title} {announcement.Description}", $"{a.Title} {a.Description}"))
                .OrderByDescending(a => a.Date)
                .Take(3)
                .ToList();

            return Ok(similar);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnnouncement(int id, Announcement announcement)
        {
            if (id != announcement.id)
            {
                return BadRequest();
            }

            _context.Entry(announcement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Announcement>> PostAnnouncement(Announcement announcement)
        {
            announcement.Title = TrimToLength(announcement.Title, 50);
            announcement.Description = TrimToLength(announcement.Description, 9999);
            _context.Announcement.Add(announcement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnnouncement", new { id = announcement.id }, announcement);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcement.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            _context.Announcement.Remove(announcement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string TrimToLength(string? input, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Trim();
            return input.Length > maxLength ? input.Substring(0, maxLength) : input;
        }
        private bool AnnouncementExists(int id)
        {
            return _context.Announcement.Any(e => e.id == id);
        }
    }
}
