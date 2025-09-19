namespace AuctionSystem.Client.Models
{
    public class CreateAuctionDto
    {
        public string Title { get; set; } = "";
        public decimal StartPrice { get; set; }
        public DateTime EndDate { get; set; }

        // Car-only fields
        public string Make { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }

        // Make VIN non-null with a safe default to avoid binding issues
        public string Vin { get; set; } = "";

        public int? MileageKm { get; set; }
        public string? Description { get; set; }
    }
}
