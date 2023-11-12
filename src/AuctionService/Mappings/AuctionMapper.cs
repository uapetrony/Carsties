namespace AuctionService;

public static class AuctionMapper
{
    public static AuctionDto ToAuctionDto(this Auction auction)
    {
        var dto = new AuctionDto
        {
            AuctionEnd = auction.AuctionEnd,
            Color = auction.Item.Color,
            CreatedAt = auction.CreatedAt,
            CurrentHighBid = auction.CurrentHighBid,
            Id = auction.Id,
            ImageUrl = auction.Item.ImageUrl,
            Make = auction.Item.Make,
            Mileage = auction.Item.Mileage,
            Model = auction.Item.Model,
            ReservePrice = auction.ReservePrice,
            Seller = auction.Seller,
            SoldAmount = auction.SoldAmount,
            Status = auction.Status.ToString(),
            UpdatedAt = auction.UpdatedAt,
            Winner = auction.Winner,
            Year = auction.Item.Year
        };

        return dto;
    }

    public static Auction ToAuction(this CreateAuctionDto dto)
    {
        var auction = new Auction
        {
            AuctionEnd = dto.AuctionEnd,
            Item = new()
            {
                Color = dto.Color,
                ImageUrl = dto.ImageUrl,
                Make = dto.Make,
                Mileage = dto.Mileage,
                Model = dto.Model,
                Year = dto.Year
            },
            ReservePrice = dto.ReservePrice
        };

        return auction;
    }
}
