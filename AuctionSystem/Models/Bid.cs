using System;
using System.Text.Json.Serialization;

namespace AuctionSystem.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

        public int AuctionId { get; set; }

        [JsonIgnore] 
        public Auction Auction { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
