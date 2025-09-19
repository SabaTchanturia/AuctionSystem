using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionSystem.Data;
using System.Security.Claims;

namespace AuctionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BidsController(AppDbContext db)
        {
            _db = db;
        }

        // Body is a plain decimal (e.g., 150), not a JSON object
        [Authorize]
        [HttpPost("{auctionId}")]
        public async Task<IActionResult> PlaceBid(int auctionId, [FromBody] decimal amount)
        {
            var auction = await _db.Auctions
                .Include(a => a.Bids)
                .FirstOrDefaultAsync(a => a.Id == auctionId);

            if (auction == null) return NotFound("Auction not found");
            if (DateTime.UtcNow > auction.EndDate) return BadRequest("Auction already ended");
            if (amount <= auction.CurrentPrice) return BadRequest("Bid must be higher than current price");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var bid = new AuctionSystem.Models.Bid
            {
                AuctionId = auctionId,
                UserId = userId,
                Amount = amount
            };

            auction.CurrentPrice = amount;
            auction.WinnerId = userId;

            _db.Bids.Add(bid);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Bid placed", currentPrice = auction.CurrentPrice, winnerId = auction.WinnerId });
        }
    }
}
