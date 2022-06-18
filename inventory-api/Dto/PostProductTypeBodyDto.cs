namespace inventory_api.Dto;

public record PostProductTypeBodyDto
{
    public string? Name { get; init; }
    public string? Description { get; init; }
}
