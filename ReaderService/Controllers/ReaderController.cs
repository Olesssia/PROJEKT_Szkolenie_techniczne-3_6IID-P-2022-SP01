using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using ReaderService.Data;
using ReaderService.Models;

namespace ReaderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadersController : ControllerBase
    {
        private readonly ReaderDbContext _context;

        public ReadersController(ReaderDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Readers.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reader reader)
        {
            _context.Readers.Add(reader);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = reader.Id }, reader);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Reader updated)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null) return NotFound();

            reader.FirstName = updated.FirstName;
            reader.LastName = updated.LastName;
            reader.Email = updated.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null) return NotFound();

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
