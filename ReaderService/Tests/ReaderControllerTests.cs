using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReaderService.Controllers;
using ReaderService.Data;
using ReaderService.Models;
using Xunit;

namespace ReaderService.Tests
{
    public class ReadersControllerTests
    {
        private ReaderDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ReaderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ReaderDbContext(options);
        }

        [Fact]
        public async Task CreateReader_ReturnsCreated()
        {
            var context = GetDbContext();
            var controller = new ReadersController(context);
            var reader = new Reader { FirstName = "Jan", LastName = "Kowalski", Email = "jan@test.com" };

            var result = await controller.Create(reader);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdReader = Assert.IsType<Reader>(createdResult.Value);
            Assert.Equal("Jan", createdReader.FirstName);
        }

        [Fact]
        public async Task GetAll_ReturnsReaders()
        {
            var context = GetDbContext();
            context.Readers.Add(new Reader { FirstName = "Anna", LastName = "Nowak", Email = "anna@test.com" });
            context.SaveChanges();

            var controller = new ReadersController(context);
            var result = await controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var readers = Assert.IsType<List<Reader>>(okResult.Value);
            Assert.Single(readers);
        }

        [Fact]
        public async Task UpdateReader_ReturnsNoContent_WhenSuccessful()
        {
            var context = GetDbContext();
            var reader = new Reader { FirstName = "Old", LastName = "Name", Email = "old@example.com" };
            context.Readers.Add(reader);
            context.SaveChanges();

            var controller = new ReadersController(context);
            var updatedReader = new Reader
            {
                Id = reader.Id,
                FirstName = "New",
                LastName = "Name",
                Email = "new@example.com"
            };

            var result = await controller.Update(reader.Id, updatedReader);

            Assert.IsType<NoContentResult>(result);

            var savedReader = context.Readers.Find(reader.Id);
            Assert.Equal("New", savedReader.FirstName);
        }

        [Fact]
        public async Task DeleteReader_ReturnsNoContent_WhenSuccessful()
        {
            var context = GetDbContext();
            var reader = new Reader { FirstName = "Delete", LastName = "Me", Email = "delete@example.com" };
            context.Readers.Add(reader);
            context.SaveChanges();

            var controller = new ReadersController(context);

            var result = await controller.Delete(reader.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Readers.Find(reader.Id));
        }
    }
}
