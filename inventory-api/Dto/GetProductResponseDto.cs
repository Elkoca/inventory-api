namespace inventory_api.Dto;

public record GetProductResponseDto
{
    public Guid ProductId { get; init; }
    public DateTime Created { get; init; }
    public DateTime LastUpdated { get; init; }
    public string? Name { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public int? Stock { get; set; }
    public int? ArticleNo { get; set; }
    //public Guid? CurrencyId { get; set; }
    public GetCurrencyResponseDto? Currency { get; set; }
    //public Guid? ProductTypeId { get; set; }
    public GetProductTypeResponseDto? ProductType { get; set; }
    //public Guid? VendorId { get; set; }
    public GetVendorResponseDto? Vendor { get; set; }
}
