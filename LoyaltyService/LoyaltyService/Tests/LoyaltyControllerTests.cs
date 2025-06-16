using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoyaltyService.Controllers;
using LoyaltyService.Data;
using LoyaltyService.Models;
using Xunit;

namespace LoyaltyService.Tests
{
    public class LoyaltyControllerTests
    {
        private LoyaltyDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<LoyaltyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new LoyaltyDbContext(options);
        }

        [Fact]
        public async Task GetPoints_ReturnsLoyalty_WhenFound()
        {
            var context = GetDbContext();
            context.Loyalties.Add(new Loyalty { ReaderId = 1, Points = 5 });
            context.SaveChanges();

            var controller = new LoyaltyController(context);
            var result = await controller.GetPoints(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var loyalty = Assert.IsType<Loyalty>(okResult.Value);
            Assert.Equal(5, loyalty.Points);
        }

        [Fact]
        public async Task GetPoints_ReturnsNotFound_WhenNotExists()
        {
            var controller = new LoyaltyController(GetDbContext());
            var result = await controller.GetPoints(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddPoints_CreatesNewLoyalty_WhenNotExists()
        {
            var context = GetDbContext();
            var controller = new LoyaltyController(context);
            var model = new Loyalty { ReaderId = 2 };

            var result = await controller.AddPoints(model);

            Assert.IsType<OkResult>(result);
            var loyalty = context.Loyalties.FirstOrDefault(l => l.ReaderId == 2);
            Assert.NotNull(loyalty);
            Assert.Equal(1, loyalty.Points);
        }

        [Fact]
        public async Task AddPoints_IncrementsPoints_WhenExists()
        {
            var context = GetDbContext();
            context.Loyalties.Add(new Loyalty { ReaderId = 3, Points = 2 });
            context.SaveChanges();

            var controller = new LoyaltyController(context);
            var result = await controller.AddPoints(new Loyalty { ReaderId = 3 });

            Assert.IsType<OkResult>(result);
            var loyalty = context.Loyalties.First(l => l.ReaderId == 3);
            Assert.Equal(3, loyalty.Points);
        }

        [Fact]
        public async Task UpdatePoints_ReturnsNoContent_WhenUpdated()
        {
            var context = GetDbContext();
            context.Loyalties.Add(new Loyalty { ReaderId = 4, Points = 5 });
            context.SaveChanges();

            var controller = new LoyaltyController(context);
            var result = await controller.UpdatePoints(4, 10);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal(10, context.Loyalties.First(l => l.ReaderId == 4).Points);
        }

        [Fact]
        public async Task UpdatePoints_ReturnsNotFound_WhenReaderNotFound()
        {
            var controller = new LoyaltyController(GetDbContext());
            var result = await controller.UpdatePoints(999, 15);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteLoyalty_RemovesRecord_WhenExists()
        {
            var context = GetDbContext();
            context.Loyalties.Add(new Loyalty { ReaderId = 5, Points = 3 });
            context.SaveChanges();

            var controller = new LoyaltyController(context);
            var result = await controller.DeleteLoyalty(5);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Loyalties.FirstOrDefault(l => l.ReaderId == 5));
        }

        [Fact]
        public async Task DeleteLoyalty_ReturnsNotFound_WhenNotExists()
        {
            var controller = new LoyaltyController(GetDbContext());
            var result = await controller.DeleteLoyalty(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
