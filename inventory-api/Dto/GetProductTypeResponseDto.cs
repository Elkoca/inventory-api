namespace inventory_api.Dto;

public class GetProductTypeResponseDto
{
    public Guid ProductTypeId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
