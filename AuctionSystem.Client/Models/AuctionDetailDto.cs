using System;
using System.Collections.Generic;

namespace AuctionSystem.Client.Models
{
    // Detailed DTO extends the listing DTO
    public class AuctionDetailDto : AuctionDto
    {
        public string? Vin { get; set; }
        public int? MileageKm { get; set; }
        public string? Description { get; set; }

        public List<BidDto> Bids { get; set; } = new();
    }
}

