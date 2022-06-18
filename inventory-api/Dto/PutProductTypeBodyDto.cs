namespace inventory_api.Dto;

public record PutProductTypeBodyDto
{
    public Guid ProductTypeId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
}
