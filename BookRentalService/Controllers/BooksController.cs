using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookRentalService.Data;
using BookRentalService.Models;

namespace BookRentalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookRentalDbContext _context;

        public BooksController(BookRentalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Books.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Books book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Books updatedBook)
        {
            if (id != updatedBook.Id)
                return BadRequest("ID mismatch");

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
                return NotFound();


            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.IsAvailable = updatedBook.IsAvailable;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

