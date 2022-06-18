using inventory_api.Extensions;
using inventory_api.Interfaces;

namespace inventory_api.Dto;

public record GetProductTypeListResponseDto : ILinkedResource
{
    public int CurrentPage { get; init; } //Current Page
    public int PageSize { get; init; }  //Current Limit
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }

    public List<GetProductTypeResponseDto> Items { get; init; }
    public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
}
