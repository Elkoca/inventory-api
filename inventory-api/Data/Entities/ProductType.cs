namespace inventory_api.Data.Entities;

public class ProductType : IEntity
{
    public Guid ProductTypeId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public List<Product>? Products { get; set; }
}
