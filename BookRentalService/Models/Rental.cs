namespace BookRentalService.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int BookId { get; set; }
        public DateTime RentedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }
    }
}
