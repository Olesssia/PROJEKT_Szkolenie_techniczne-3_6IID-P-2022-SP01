using Microsoft.EntityFrameworkCore;
using ReaderService.Models;
using System.Collections.Generic;

namespace ReaderService.Data
{
    public class ReaderDbContext : DbContext
    {
        public ReaderDbContext(DbContextOptions<ReaderDbContext> options) : base(options) { }

    public DbSet<Reader> Readers => Set<Reader>();
}
}
