namespace inventory_api.Dto;

public record PostVendorBodyDto
{
    public string? Name { get; init; }
    public Uri? WebSite { get; init; }
}
