using System.ComponentModel.DataAnnotations;

namespace AuctionService;

public class UpdateAuctionDto
{
    [StringLength(64, MinimumLength = 2)]
    public string? Color { get; set; }
    [StringLength(64, MinimumLength = 2)]
    public string? Make { get; set; }
    [StringLength(64, MinimumLength = 2)]
    public string? Model { get; set; }
    [Range(1900, 2099)]
    public int? Year { get; set; }
    [Range(0, int.MaxValue)]
    public int? Mileage { get; set; }
}
