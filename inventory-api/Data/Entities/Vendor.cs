namespace inventory_api.Data.Entities;

public class Vendor : IEntity
{
    public Guid VendorId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public string? Name { get; set; }
    public Uri? WebSite { get; set; }

    public List<Product>? Products { get; set; }
}
