using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

[ApiController]
[Route("api/auctions")]
public class AuctionsController: ControllerBase
{
    private readonly AuctionDbContext _context;

    public AuctionsController(AuctionDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
    {
        var query = _context.Auctions
            .OrderBy(x => x.Item.Make)
            .AsQueryable();

        if (DateTime.TryParse(date, out var parsedDate))
        {
            var utcDate = parsedDate.ToUniversalTime();
            query = query.Where(x => x.UpdatedAt.CompareTo(utcDate) > 0);
        }

        var auctions = await query
            .Include(x => x.Item)
            .Select(x => x.ToAuctionDto())
            .ToListAsync();

        return auctions;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

        if (auction is null) {
            return NotFound();
        }

        return auction.ToAuctionDto();
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto dto)
    {
        var auction = dto.ToAuction();
        // TODO: add current user as seller
        auction.Seller = "test";

        _context.Auctions.Add(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) 
        {
            return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, auction.ToAuctionDto());
        }

        return BadRequest("Could not save changes to the DB");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto dto)
    {
        var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

        if (auction is null)
        {
            return NotFound();
        }

        var result = await UpdateAuctionFromDto(auction, dto) > 0;

        if (result)
        {
            return Ok();
        }

        return BadRequest($"Auction {id} was not changed");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id) 
    {
        var auction = await _context.Auctions.FindAsync(id);

        if (auction is null )
        {
            return NotFound();
        }

        _context.Auctions.Remove(auction);
        var result = await _context.SaveChangesAsync() > 0;

        if (!result)
        {
            return BadRequest("Could not update DB");
        }

        return Ok();
    }

    private Task<int> UpdateAuctionFromDto(Auction auction, UpdateAuctionDto dto)
    {
        // TODO: seller == username
        if (dto.Color is not null && auction.Item.Color != dto.Color)
        {
            auction.Item.Color = dto.Color;
        }

        if (dto.Make is not null && auction.Item.Make != dto.Make)
        {
            auction.Item.Make = dto.Make;
        }

        if (dto.Model is not null && auction.Item.Model != dto.Model)
        {
            auction.Item.Model = dto.Model;
        }

        if (dto.Mileage is not null && auction.Item.Mileage != dto.Mileage)
        {
            auction.Item.Mileage = dto.Mileage ?? 0;
        }

        if (dto.Year is not null && auction.Item.Year != dto.Year)
        {
            auction.Item.Year = dto.Year ?? 0;
        }

        return _context.SaveChangesAsync();
    }
}
