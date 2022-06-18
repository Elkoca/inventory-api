namespace inventory_api.Dto;

public class GetVendorResponseDto
{
    public Guid VendorId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public string? Name { get; set; }
    public Uri? WebSite { get; set; }
}
