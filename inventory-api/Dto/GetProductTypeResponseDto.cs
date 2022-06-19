namespace inventory_api.Dto;

public record GetProductTypeResponseDto
{
    public Guid ProductTypeId { get; init; }
    public DateTime Created { get; init; }
    //public DateTime LastUpdated { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}
