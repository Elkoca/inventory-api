using AutoMapper;
using inventory_api.Dto;
using inventory_api.Extensions;
using inventory_api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace inventory_api.Controllers;

//[Route("api/Products/{ProductId}/[controller]")]
[Route("api/[controller]")]
[ApiController]
public class VendorsController : ControllerBase
{
    private readonly IVendorService _service;
    public readonly IMapper _mapper;

    public VendorsController(IVendorService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet(Name = nameof(GetVendorListAsync))]
    [ProducesResponseType(typeof(GetVendorListResponseDto), Status200OK)]
    public async Task<IActionResult> GetVendorListAsync([FromQuery] UrlQueryGetVendorListDto urlQueryParameters, CancellationToken cancellationToken)
    {
        var vendors = await _service.GetByPageAsync(
                            urlQueryParameters.Limit,
                            urlQueryParameters.Page,
                            urlQueryParameters.SortBy,
                            cancellationToken);

        return Ok(GeneratePageLinks(urlQueryParameters, vendors));
    }

    //Flyttes senere
    private GetVendorListResponseDto GeneratePageLinks(UrlQueryPagingBaseDto queryParameters, GetVendorListResponseDto response)
    {
        if (response.TotalPages > 1)
        {
            //First
            var firstRoute = Url.RouteUrl(nameof(GetVendorListAsync), new { limit = queryParameters.Limit, page = 1 });
            if(firstRoute != null) 
                response.AddResourceLink(LinkedResourceType.First, firstRoute);
            //Last
            var lastRoute = Url.RouteUrl(nameof(GetVendorListAsync), new { limit = queryParameters.Limit, page = response.TotalPages });
            if (lastRoute != null)
                response.AddResourceLink(LinkedResourceType.Last, lastRoute);
        }

        //Prev (If exist)
        if (response.CurrentPage > 1)
        {
            var prevRoute = Url.RouteUrl(nameof(GetVendorListAsync), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });
            if (prevRoute != null)
                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);
        }
        //next (If exist (current page is not last))
        if (response.CurrentPage < response.TotalPages)
        {
            var nextRoute = Url.RouteUrl(nameof(GetVendorListAsync), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });
            if (nextRoute != null)
                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
        }


        return response;
    }

}
