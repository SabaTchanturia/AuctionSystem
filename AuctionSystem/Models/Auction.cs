using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;                    // [JsonIgnore]
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;  // [ValidateNever]

namespace AuctionSystem.Models
{
    public class Auction
    {
        public int Id { get; set; }

        // General
        public string Title { get; set; } = "";
        public decimal StartPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime EndDate { get; set; }

        // Winner (last highest bidder)
        public int? WinnerId { get; set; }
        public User? Winner { get; set; }

        // ✅ Car-only fields
        public string Make { get; set; } = "";    // e.g., Toyota
        public string Model { get; set; } = "";   // e.g., Corolla
        public int Year { get; set; }             // e.g., 2016
        public string? Vin { get; set; }          // optional
        public int? MileageKm { get; set; }       // optional (km)
        public string? Description { get; set; }  // optional



        // Creator (FK is required; navigation is nullable to avoid model-binding validation)
        public int CreatedByUserId { get; set; }

        [JsonIgnore]                 // don’t serialize navigation to avoid cycles
        [ValidateNever]              // don’t require it from the incoming JSON
        public User? CreatedByUser { get; set; }  // ← მხოლოდ ეს ერთი დატოვე

        // Soft delete flag
        public bool IsDeleted { get; set; } = false;

        [JsonIgnore]                 // avoid cycles on serialization
        [ValidateNever]
        public List<Bid> Bids { get; set; } = new();
    }
}