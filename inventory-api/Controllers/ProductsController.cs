using inventory_api.Dto;
using inventory_api.Interfaces;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using inventory_api.Extensions;

namespace inventory_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet(Name = nameof(GetProductListAsync))]
    [ProducesResponseType(typeof(GetProductListResponseDto), Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), Status400BadRequest)]
    public async Task<IActionResult> GetProductListAsync([FromQuery] UrlQueryProductListDto urlQueryParameters, CancellationToken cancellationToken)
    {
        //Tror ikke jeg trenger denne lenger
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var products = await _service.GetByPageAsync(
                                urlQueryParameters.Limit,
                                urlQueryParameters.Page,
                                cancellationToken);

        return Ok(GeneratePageLinks(urlQueryParameters, products));
    }

    private GetProductListResponseDto GeneratePageLinks(UrlQueryBaseDto queryParameters, GetProductListResponseDto response)
    {
        if (response.TotalPages > 1)
        {
            //First
            var firstRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = 1 });
            response.AddResourceLink(LinkedResourceType.First, firstRoute);

            //Last
            var lastRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = response.TotalPages });
            response.AddResourceLink(LinkedResourceType.Last, lastRoute);
        }

        //Prev (If exist)
        if (response.CurrentPage > 1)
        {
            var prevRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });

            response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

        }

        //next (If exist (current page is not last))
        if (response.CurrentPage < response.TotalPages)
        {
            var nextRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });

            response.AddResourceLink(LinkedResourceType.Next, nextRoute);
        }


        return response;
    }
}
