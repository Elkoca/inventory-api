namespace inventory_api.Dto;

public record GetProductResponseDto
{
    public Guid ProductId { get; init; }
    public DateTime Created { get; init; }
    //public DateTime LastUpdated { get; init; }
    public string? Name { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int Stock { get; init; }
    public int? ArticleNo { get; init; }
    //public Guid? PriceId { get; set; }
    public GetPriceResponseDto? Price { get; init; }
    public Guid? ProductTypeId { get; init; }
    public GetProductTypeResponseDto? ProductType { get; set; }
    public Guid? VendorId { get; init; }
    public GetVendorResponseDto? Vendor { get; set; }
}
