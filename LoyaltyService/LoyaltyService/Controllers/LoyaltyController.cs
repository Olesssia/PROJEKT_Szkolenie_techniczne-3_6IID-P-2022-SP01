using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoyaltyService.Data;
using LoyaltyService.Models;

namespace LoyaltyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoyaltyController : ControllerBase
    {
        private readonly LoyaltyDbContext _context;

        public LoyaltyController(LoyaltyDbContext context)
        {
            _context = context;
        }

        [HttpGet("{readerId}")]
        public async Task<IActionResult> GetPoints(int readerId)
        {
            var loyalty = await _context.Loyalties.FirstOrDefaultAsync(l => l.ReaderId == readerId);
            if (loyalty == null) return NotFound();
            return Ok(loyalty);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPoints([FromBody] Loyalty model)
        {
            var loyalty = await _context.Loyalties.FirstOrDefaultAsync(l => l.ReaderId == model.ReaderId);
            if (loyalty == null)
            {
                model.Points = 1;
                _context.Loyalties.Add(model);
            }
            else
            {
                loyalty.Points += 1;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{readerId}")]
        public async Task<IActionResult> UpdatePoints(int readerId, [FromBody] int newPoints)
        {
            var loyalty = await _context.Loyalties.FirstOrDefaultAsync(l => l.ReaderId == readerId);
            if (loyalty == null) return NotFound();

            loyalty.Points = newPoints;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{readerId}")]
        public async Task<IActionResult> DeleteLoyalty(int readerId)
        {
            var loyalty = await _context.Loyalties.FirstOrDefaultAsync(l => l.ReaderId == readerId);
            if (loyalty == null) return NotFound();

            _context.Loyalties.Remove(loyalty);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}