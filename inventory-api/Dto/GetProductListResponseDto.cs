using inventory_api.Extensions;
using inventory_api.Interfaces;

namespace inventory_api.Dto;

public record GetProductListResponseDto : ILinkedResource
{
    public int CurrentPage { get; init; } //Current Page
    public int PageSize { get; init; }  //Current Limit
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }

    public List<GetProductResponseDto> Items { get; init; }
    public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
}
