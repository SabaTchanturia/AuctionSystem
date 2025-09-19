namespace AuctionSystem.Client.Models
{
    // Lightweight listing DTO for /auctions
    public class AuctionDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public decimal CurrentPrice { get; set; }
        public DateTime EndDate { get; set; }

        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }

        // NEW: who created this auction (used only for permissions on the client)
        public int CreatedByUserId { get; set; }

        // Winner (optional)
        public int? WinnerId { get; set; }
        public UserDto? Winner { get; set; }
    }
}
