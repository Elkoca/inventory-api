using inventory_api.Extensions;

namespace inventory_api.Interfaces;

public interface ILinkedResource
{
    public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
}
