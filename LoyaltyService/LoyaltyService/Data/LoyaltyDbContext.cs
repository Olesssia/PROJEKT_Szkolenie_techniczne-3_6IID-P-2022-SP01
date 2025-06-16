using Microsoft.EntityFrameworkCore;
using LoyaltyService.Models;

namespace LoyaltyService.Data
{
    public class LoyaltyDbContext : DbContext
    {
        public LoyaltyDbContext(DbContextOptions<LoyaltyDbContext> options) : base(options) { }

        public DbSet<Loyalty> Loyalties => Set<Loyalty>();
    }
}
