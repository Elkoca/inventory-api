using System.ComponentModel.DataAnnotations;

namespace inventory_api.Dto;

public record PostProductBodyDto
{
    [Required]
    public string Name { get; init; }
    [Required]
    public string Title { get; init; }
    public string? Description { get; init; }
}
