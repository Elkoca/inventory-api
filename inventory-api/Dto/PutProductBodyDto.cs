using System.ComponentModel.DataAnnotations;

namespace inventory_api.Dto;

public record PutProductBodyDto
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    public string Name { get; init; }
    [Required]
    public string Title { get; init; }
    public string? Description { get; init; }
}
