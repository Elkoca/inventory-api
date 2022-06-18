using inventory_api.Dto;
using inventory_api.Interfaces;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using inventory_api.Extensions;
using System.Reflection;
using AutoMapper;

namespace inventory_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    public readonly IMapper _mapper;

    public ProductsController(IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet(Name = nameof(GetProductListAsync))]
    [ProducesResponseType(typeof(GetProductListResponseDto), Status200OK)]
    public async Task<IActionResult> GetProductListAsync([FromQuery] UrlQueryGetProductListDto urlQueryParameters, CancellationToken cancellationToken)
    {
            var products = await _service.GetByPageAsync(
                                urlQueryParameters.Limit,
                                urlQueryParameters.Page,
                                urlQueryParameters.SortBy,
                                cancellationToken);

        return Ok(GeneratePageLinks(urlQueryParameters, products));

    }
    [HttpGet("{ProductId}", Name = nameof(GetProductAsync))]
    [ProducesResponseType(typeof(GetProductResponseDto), Status200OK)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> GetProductAsync([FromRoute] Guid ProductId, CancellationToken cancellationToken)
    {
        var product = await _service.GetByIdAsync(ProductId, cancellationToken);

        if (product == null)
            return NotFound($"product with id {ProductId}, was not found");

        return Ok(product);
    }

    [HttpPost(Name = nameof(PostProductAsync))]
    [ProducesResponseType(typeof(GetProductResponseDto), Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PostProductAsync([FromBody] PostProductBodyDto newProuduct, CancellationToken cancellationToken)
    {
        GetProductResponseDto cratedProduct = await _service.CreateAsync(newProuduct, cancellationToken);
        
        return CreatedAtAction("GetProduct", new { ProductId = cratedProduct.ProductId }, cratedProduct);
    }

    [HttpPut("{ProductId}", Name = nameof(PutProductAsync))]
    [ProducesResponseType(typeof(GetProductResponseDto), Status201Created)]
    [ProducesResponseType(Status204NoContent)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PutProductAsync([FromRoute] Guid ProductId, [FromBody] PutProductBodyDto product, CancellationToken cancellationToken)
    {
        if (ProductId != product.ProductId)
        {
            return BadRequest("Route, and body id is not matching");
        }

        if (!await _service.ExistAsync(ProductId, cancellationToken))
        {
            var newProduct = _mapper.Map<PostProductBodyDto>(product);
            GetProductResponseDto cratedProduct = await _service.CreateWithIdAsync(ProductId, newProduct, cancellationToken);
            return CreatedAtAction("GetProduct", new { ProductId = cratedProduct.ProductId }, cratedProduct);
        }

        await _service.ReplaceAsync(product, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{ProductId}", Name = nameof(DeleteProductAsync))]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid ProductId, CancellationToken cancellationToken)
    {
        //idempotent - 204 uansett om den eksisterer eller ikke 
        if(await _service.ExistAsync(ProductId, cancellationToken));
            await _service.DeleteAsync(ProductId, cancellationToken);

        return NoContent();
    }

    //Flyttes senere
    private GetProductListResponseDto GeneratePageLinks(UrlQueryPagingBaseDto queryParameters, GetProductListResponseDto response)
    {
        if (response.TotalPages > 1)
        {
            //First
            var firstRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = 1 });
            if (firstRoute != null)
                response.AddResourceLink(LinkedResourceType.First, firstRoute);

            //Last
            var lastRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = response.TotalPages });
            if (lastRoute != null)
                response.AddResourceLink(LinkedResourceType.Last, lastRoute);
        }

        //Prev (If exist)
        if (response.CurrentPage > 1)
        {
            var prevRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = queryParameters.Page - 1 });
            if (prevRoute != null)
                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

        }

        //next (If exist (current page is not last))
        if (response.CurrentPage < response.TotalPages)
        {
            var nextRoute = Url.RouteUrl(nameof(GetProductListAsync), new { limit = queryParameters.Limit, page = queryParameters.Page + 1 });
            if (nextRoute != null)
                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
        }


        return response;
    }
}
