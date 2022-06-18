namespace inventory_api.Dto;

public class GetCurrencyResponseDto
{
    public Guid CurrencyId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public string? Code { get; set; } // USD, NOK
}
