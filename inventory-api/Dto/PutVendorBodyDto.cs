namespace inventory_api.Dto;

public record PutVendorBodyDto
{
    public Guid VendorId { get; init; }
    public string? Name { get; init; }
    public Uri? WebSite { get; init; }
}
