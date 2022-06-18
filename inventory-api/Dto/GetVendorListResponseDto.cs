using inventory_api.Extensions;
using inventory_api.Interfaces;

namespace inventory_api.Dto;

public record GetVendorListResponseDto : ILinkedResource
{
    public int CurrentPage { get; init; } //Current Page
    public int PageSize { get; init; }  //Current Limit
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }

    public List<GetVendorResponseDto> Items { get; init; }
    public IDictionary<LinkedResourceType, LinkedResource> Links { get; set; }
}
