namespace inventory_api.Dto;

public record GetProductResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Title { get; init; }
    public string? Description { get; init; }
}
