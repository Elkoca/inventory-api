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
                            urlQueryParameters.Search,
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
    public async Task<IActionResult> PostProductAsync([FromBody] PostProductBodyDto newProduct, CancellationToken cancellationToken)
    {
        GetProductResponseDto createdProduct = await _service.CreateAsync(newProduct, cancellationToken);
        
        return CreatedAtAction("GetProduct", new { ProductId = createdProduct.ProductId }, createdProduct);
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
            GetProductResponseDto createdProduct = await _service.CreateWithIdAsync(ProductId, newProduct, cancellationToken);
            return CreatedAtAction("GetProduct", new { ProductId = createdProduct.ProductId }, createdProduct);
        }

        await _service.ReplaceAsync(product, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{ProductId}", Name = nameof(DeleteProductAsync))]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid ProductId, CancellationToken cancellationToken)
    {
        //idempotent - 204 uansett om den eksisterer eller ikke 
        if (await _service.ExistAsync(ProductId, cancellationToken))
            await _service.DeleteAsync(ProductId, cancellationToken);

        return NoContent();
    }

    //Flyttes senere
    private GetProductListResponseDto GeneratePageLinks(UrlQueryPagingBaseDto queryParameters, GetProductListResponseDto response, string? search = null)
    {
        var usedQueryParams = new UsedQueryParameters();
        usedQueryParams.Limit = queryParameters.Limit;
        usedQueryParams.SortBy = queryParameters.SortBy;
        usedQueryParams.Search = search;

        if (response.TotalPages > 1)
        {
            //First
            usedQueryParams.Page = 1;
            var firstRoute = Url.RouteUrl(nameof(GetProductListAsync), usedQueryParams);
            if (firstRoute != null)
                response.AddResourceLink(LinkedResourceType.First, firstRoute);

            //Last
            usedQueryParams.Page = response.TotalPages;
            var lastRoute = Url.RouteUrl(nameof(GetProductListAsync), usedQueryParams);
            if (lastRoute != null)
                response.AddResourceLink(LinkedResourceType.Last, lastRoute);
        }

        //Prev (If exist)
        if (response.CurrentPage > 1)
        {
            usedQueryParams.Page = queryParameters.Page - 1;
            var prevRoute = Url.RouteUrl(nameof(GetProductListAsync), usedQueryParams);
            if (prevRoute != null)
                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

        }

        //next (If exist (current page is not last))
        if (response.CurrentPage < response.TotalPages)
        {
            usedQueryParams.Page = queryParameters.Page + 1;
            var nextRoute = Url.RouteUrl(nameof(GetProductListAsync), usedQueryParams);
            if (nextRoute != null)
                response.AddResourceLink(LinkedResourceType.Next, nextRoute);
        }


        return response;
    }

    private struct UsedQueryParameters
    {
        public int Limit { get; set; }
        public int Page { get; set; }
        public string? SortBy { get; set; }
        public string? Search { get; set; }
    }
}
