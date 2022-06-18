using System.ComponentModel.DataAnnotations;

namespace inventory_api.Dto;

public record PostProductBodyDto
{
    [Required]
    public string Name { get; init; }
    [Required]
    public string Title { get; init; }
    public string? Description { get; init; }
    [Required]
    public int Stock { get; init; } //Antal varer
    public int? ArticleNo { get; init; } //Barcode
    public PostPriceBodyDto? Price { get; init; }
    public Guid? ProductTypeId { get; init; }
    //public PostProductTypeBodyDto? ProductType { get; init; }
    public Guid? ProductVendorId { get; init; }
    //public PostVendorBodyDto? Vendor { get; init; }
}
