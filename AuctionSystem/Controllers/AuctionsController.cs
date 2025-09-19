using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionSystem.Data;
using AuctionSystem.Models;
using System.Linq; // FirstOrDefault, OrderByDescending
using System;     // DateTime

namespace AuctionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AuctionsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var auctions = await _db.Auctions
                .Include(a => a.Winner) // optional winner info
                .ToListAsync();

            return Ok(auctions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var auction = await _db.Auctions
                .Include(a => a.Bids).ThenInclude(b => b.User)
                .Include(a => a.Winner)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auction == null) return NotFound();

            var dto = new
            {
                auction.Id,
                auction.Title,
                auction.CurrentPrice,
                auction.EndDate,
                auction.Make,
                auction.Model,
                auction.Year,
                auction.Vin,
                auction.MileageKm,
                auction.Description,
                auction.CreatedByUserId,
                auction.WinnerId,
                Winner = auction.Winner == null ? null : new
                {
                    auction.Winner.Id,
                    auction.Winner.Username
                },
                Bids = auction.Bids
                    .OrderByDescending(b => b.PlacedAt)
                    .Select(b => new
                    {
                        b.Id,
                        b.Amount,
                        b.PlacedAt,
                        b.UserId,
                        User = b.User == null ? null : new
                        {
                            b.User.Id,
                            b.User.Username
                        }
                    })
                    .ToList()
            };

            return Ok(dto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Auction auction)
        {
            if (auction.StartPrice < 0) return BadRequest("StartPrice must be >= 0.");
            if (auction.EndDate <= DateTime.UtcNow) return BadRequest("EndDate must be in the future (UTC).");

            if (string.IsNullOrWhiteSpace(auction.Make)) return BadRequest("Make is required (e.g., Toyota).");
            if (string.IsNullOrWhiteSpace(auction.Model)) return BadRequest("Model is required (e.g., Corolla).");
            if (auction.Year < 1980 || auction.Year > DateTime.UtcNow.Year + 2)
                return BadRequest($"Year must be between 1980 and {DateTime.UtcNow.Year + 2}.");

            // თუ Title ცარიელია — ავტომატურად ავაწყოთ
            if (string.IsNullOrWhiteSpace(auction.Title))
                auction.Title = $"{auction.Make} {auction.Model} {auction.Year}";

            // დაწყებისას მიმდინარე ფასი = საწყისი ფასი
            auction.CurrentPrice = auction.StartPrice;

            // ✅ JWT-დან ამოვიღოთ იუზერის ID და ჩავსვათ მფლობელად
            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type.EndsWith("/nameidentifier") || c.Type.Contains("nameidentifier"));
            if (userIdClaim == null) return Unauthorized();
            if (!int.TryParse(userIdClaim.Value, out var userId)) return Unauthorized();

            auction.CreatedByUserId = userId;
            auction.IsDeleted = false; // just explicit

            _db.Auctions.Add(auction);
            await _db.SaveChangesAsync();

            // დააბრუნე ბოლო ვერსია (OK ან Created)
            return Ok(auction);
            // სურვილისამებრ:
            // return CreatedAtAction(nameof(Get), new { id = auction.Id }, auction);
        }

        // DELETE: api/Auctions/5
        // SOFT DELETE — მხოლოდ ის იუზერი წაშლის, ვინც შექმნა (CreatedByUserId). Admin ლოგიკა შეგიძლიათ დამატოთ მოგვიანებით.
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var auction = await _db.Auctions.FindAsync(id);
            if (auction == null) return NotFound();

            // ამოვიღოთ მიმდინარე იუზერის Id ტოკენიდან
            var userIdClaim = User.Claims.FirstOrDefault(c =>
                c.Type.EndsWith("/nameidentifier") || c.Type.Contains("nameidentifier"));
            if (userIdClaim == null) return Unauthorized();
            if (!int.TryParse(userIdClaim.Value, out var userId)) return Unauthorized();

            // მხოლოდ შემქმნელს შეუძლია წაშლა
            if (auction.CreatedByUserId != userId)
                return Forbid(); // 403

            // Soft delete: რეალურად არ ვშლით, მხოლოდ ვნიშნავთ
            auction.IsDeleted = true;
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
