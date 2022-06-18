namespace inventory_api.Data.Entities;

public interface IEntity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
}
