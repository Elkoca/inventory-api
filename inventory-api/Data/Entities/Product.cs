using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_api.Data.Entities;

public class Product : IEntity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

}