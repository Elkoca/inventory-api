namespace inventory_api.Dto;

public record PutPriceBodyDto
{
    public string? Code { get; init; } // USD, NOK
    public double? Amount { get; init; } // 4999,99
}
