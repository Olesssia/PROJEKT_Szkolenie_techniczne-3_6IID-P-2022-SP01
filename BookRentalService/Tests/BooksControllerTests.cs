using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookRentalService.Controllers;
using BookRentalService.Data;
using BookRentalService.Models;
using Xunit;

namespace BookRentalService.Tests
{
    public class BooksControllerTests
    {
        private BookRentalDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BookRentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new BookRentalDbContext(options);
        }

        [Fact]
        public async Task GetAll_ReturnsBooks()
        {
            var context = GetDbContext();
            context.Books.Add(new Books { Title = "Test Book", Author = "Author", IsAvailable = true });
            context.SaveChanges();

            var controller = new BooksController(context);
            var result = await controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var books = Assert.IsType<List<Books>>(okResult.Value);
            Assert.Single(books);
        }

        [Fact]
        public async Task Create_AddsBookAndReturnsCreated()
        {
            var context = GetDbContext();
            var controller = new BooksController(context);
            var book = new Books { Title = "New Book", Author = "Author", IsAvailable = true };

            var result = await controller.Create(book);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdBook = Assert.IsType<Books>(createdResult.Value);
            Assert.Equal("New Book", createdBook.Title);
            Assert.Equal(1, context.Books.Count());
        }

        [Fact]
        public async Task Update_UpdatesBook_WhenExists()
        {
            var context = GetDbContext();
            var book = new Books { Title = "Old", Author = "Author", IsAvailable = false };
            context.Books.Add(book);
            context.SaveChanges();

            var controller = new BooksController(context);
            var updated = new Books { Id = book.Id, Title = "Updated", Author = "New Author", IsAvailable = true };

            var result = await controller.Update(book.Id, updated);

            Assert.IsType<NoContentResult>(result);
            var updatedBook = context.Books.Find(book.Id);
            Assert.Equal("Updated", updatedBook.Title);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            var controller = new BooksController(GetDbContext());
            var book = new Books { Id = 1, Title = "Book", Author = "Author", IsAvailable = true };

            var result = await controller.Update(999, book);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenBookMissing()
        {
            var controller = new BooksController(GetDbContext());
            var book = new Books { Id = 42, Title = "Book", Author = "Author", IsAvailable = true };

            var result = await controller.Update(42, book);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_RemovesBook_WhenExists()
        {
            var context = GetDbContext();
            var book = new Books { Title = "To Delete", Author = "Author", IsAvailable = true };
            context.Books.Add(book);
            context.SaveChanges();

            var controller = new BooksController(context);
            var result = await controller.Delete(book.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Books.Find(book.Id));
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenBookMissing()
        {
            var controller = new BooksController(GetDbContext());
            var result = await controller.Delete(123);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
