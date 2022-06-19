namespace inventory_api.Dto;

public record GetVendorResponseDto
{
    public Guid VendorId { get; init; }
    public DateTime Created { get; init; }
    //public DateTime LastUpdated { get; init; }

    public string? Name { get; init; }
    public Uri? WebSite { get; init; }
}
