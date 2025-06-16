using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookRentalService.Controllers;
using BookRentalService.Data;
using BookRentalService.Models;
using Xunit;

namespace BookRentalService.Tests
{
    public class RentalsControllerTests
    {
        private BookRentalDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BookRentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new BookRentalDbContext(options);
        }

        [Fact]
        public async Task Rent_BookAvailable_AddsRental()
        {
            var context = GetDbContext();
            var book = new Books { Title = "Available Book", Author = "Author", IsAvailable = true };
            context.Books.Add(book);
            context.SaveChanges();

            var rental = new Rental { BookId = book.Id, ReaderId = 1, RentedAt = DateTime.UtcNow };
            var controller = new RentalsController(context);

            var result = await controller.Rent(rental);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRental = Assert.IsType<Rental>(okResult.Value);
            Assert.Equal(book.Id, returnedRental.BookId);

            var updatedBook = context.Books.Find(book.Id);
            Assert.False(updatedBook.IsAvailable);
        }

        [Fact]
        public async Task Rent_BookUnavailable_ReturnsBadRequest()
        {
            var context = GetDbContext();
            var book = new Books { Title = "Unavailable Book", Author = "Author", IsAvailable = false };
            context.Books.Add(book);
            context.SaveChanges();

            var rental = new Rental { BookId = book.Id, ReaderId = 1, RentedAt = DateTime.UtcNow };
            var controller = new RentalsController(context);

            var result = await controller.Rent(rental);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Book is not available.", badRequest.Value);
        }

        [Fact]
        public async Task Return_ValidRental_MarksReturnedAndBookAvailable()
        {
            var context = GetDbContext();
            var book = new Books { Title = "Book", Author = "Author", IsAvailable = false };
            context.Books.Add(book);
            context.SaveChanges();

            var rental = new Rental { BookId = book.Id, ReaderId = 1, RentedAt = DateTime.UtcNow };
            context.Rentals.Add(rental);
            context.SaveChanges();

            var controller = new RentalsController(context);
            var result = await controller.Return(rental.Id);

            Assert.IsType<NoContentResult>(result);

            var updatedRental = context.Rentals.Find(rental.Id);
            Assert.NotNull(updatedRental.ReturnedAt);

            var updatedBook = context.Books.Find(book.Id);
            Assert.True(updatedBook.IsAvailable);
        }

        [Fact]
        public async Task Return_RentalNotFoundOrAlreadyReturned_ReturnsNotFound()
        {
            var context = GetDbContext();
            var rental = new Rental { BookId = 1, ReaderId = 1, RentedAt = DateTime.UtcNow, ReturnedAt = DateTime.UtcNow };
            context.Rentals.Add(rental);
            context.SaveChanges();

            var controller = new RentalsController(context);
            var result = await controller.Return(rental.Id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByUser_ReturnsRentals()
        {
            var context = GetDbContext();
            context.Rentals.Add(new Rental { BookId = 1, ReaderId = 10, RentedAt = DateTime.UtcNow });
            context.Rentals.Add(new Rental { BookId = 2, ReaderId = 10, RentedAt = DateTime.UtcNow });
            context.Rentals.Add(new Rental { BookId = 3, ReaderId = 20, RentedAt = DateTime.UtcNow });
            context.SaveChanges();

            var controller = new RentalsController(context);
            var result = await controller.GetByUser(10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentals = Assert.IsType<List<Rental>>(okResult.Value);
            Assert.Equal(2, rentals.Count);
        }
    }
}

