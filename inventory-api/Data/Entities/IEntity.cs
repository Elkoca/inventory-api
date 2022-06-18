namespace inventory_api.Data.Entities;

public interface IEntity
{
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
}
