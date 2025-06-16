using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookRentalService.Data;
using BookRentalService.Models;

namespace BookRentalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly BookRentalDbContext _context;

        public RentalsController(BookRentalDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Rent([FromBody] Rental rental)
        {
            var book = await _context.Books.FindAsync(rental.BookId);
            if (book == null || !book.IsAvailable)
                return BadRequest("Book is not available.");

            book.IsAvailable = false;
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok(rental);
        }

        [HttpPut("return/{id}")]
        public async Task<IActionResult> Return(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null || rental.ReturnedAt != null)
                return NotFound();

            rental.ReturnedAt = DateTime.UtcNow;
            var book = await _context.Books.FindAsync(rental.BookId);
            if (book != null) book.IsAvailable = true;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("user/{readerId}")]
        public async Task<IActionResult> GetByUser(int readerId)
        {
            var rentals = await _context.Rentals
                .Where(r => r.ReaderId == readerId)
                .ToListAsync();
            return Ok(rentals);
        }
    }
}