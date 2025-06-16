namespace LoyaltyService.Models
{
    public class Loyalty
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int Points { get; set; } = 0;
    }
}


