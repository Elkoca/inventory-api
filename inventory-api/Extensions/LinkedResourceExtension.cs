using inventory_api.Interfaces;

namespace inventory_api.Extensions
{
    public static class LinkedResourceExtension
    {
        public static void AddResourceLink(this ILinkedResource resources, LinkedResourceType resourceType, string routeUrl)
        {
            //If links != null, create dictionary
            resources.Links ??= new Dictionary<LinkedResourceType, LinkedResource>();
            resources.Links[resourceType] = new LinkedResource(routeUrl);
        }
    }

    public record LinkedResource(string Href);

    public enum LinkedResourceType
    {
        None,

        First,
        Last,

        Prev,
        Next
    }
}
