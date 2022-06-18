namespace inventory_api.Dto;

public record GetPriceResponseDto
{
    //public Guid PriceId { get; set; }
    //public DateTime Created { get; set; }
    //public DateTime LastUpdated { get; set; }

    public string? Code { get; init; } // USD, NOK
    public double Amount { get; init; } // 4999,99
}
