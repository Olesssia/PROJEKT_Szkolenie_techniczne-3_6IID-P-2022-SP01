using Microsoft.EntityFrameworkCore;
using BookRentalService.Models;

namespace BookRentalService.Data
{
    public class BookRentalDbContext : DbContext
    {
        public BookRentalDbContext(DbContextOptions<BookRentalDbContext> options) : base(options) { }

        public DbSet<Books> Books => Set<Books>();
        public DbSet<Rental> Rentals => Set<Rental>();
    }
}