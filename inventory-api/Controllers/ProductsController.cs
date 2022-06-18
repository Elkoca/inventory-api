using inventory_api.Dto;
using inventory_api.Interfaces;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Microsoft.AspNetCore.Mvc;
using inventory_api.Extensions;
using System.Reflection;

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
    public async Task<IActionResult> GetProductListAsync([FromQuery] UrlQueryGetProductListDto urlQueryParameters, CancellationToken cancellationToken)
    {
            var products = await _service.GetByPageAsync(
                                urlQueryParameters.Limit,
                                urlQueryParameters.Page,
                                urlQueryParameters.SortBy,
                                cancellationToken);

        return Ok(GeneratePageLinks(urlQueryParameters, products));

    }
    [HttpGet("{Id}", Name = nameof(GetProductAsync))]
    [ProducesResponseType(typeof(GetProductResponseDto), Status200OK)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> GetProductAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var product = await _service.GetByIdAsync(id, cancellationToken);

        if (product == null)
            return NotFound($"product with id {id}, was not found");

        return Ok(product);
    }

    [HttpPost(Name = nameof(PostProductAsync))]
    [ProducesResponseType(typeof(GetProductResponseDto), Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PostProductAsync([FromBody] PostProductBodyDto newProuduct, CancellationToken cancellationToken)
    {
        GetProductResponseDto cratedProduct = await _service.CreateAsync(newProuduct, cancellationToken);
        
        return CreatedAtAction("GetProduct", new { id = cratedProduct.Id }, cratedProduct);
    }

    [HttpPut("{id}", Name = nameof(PutProductAsync))]
    [ProducesResponseType(typeof(GetProductResponseDto), Status201Created)]
    [ProducesResponseType(Status204NoContent)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PutProductAsync([FromRoute] Guid id, [FromBody] PutProductBodyDto product, CancellationToken cancellationToken)
    {
        if (id != product.Id)
        {
            return BadRequest("Route, and body id is not matching");
        }

        if (!await _service.ExistAsync(id, cancellationToken))
        {
            var newProduct = new PostProductBodyDto
            {
                Name = product.Name,
                Description = product.Description,
                Title = product.Title
            };

            GetProductResponseDto cratedProduct = await _service.CreateWithIdAsync(id, newProduct, cancellationToken);
            return CreatedAtAction("GetProduct", new { id = cratedProduct.Id }, cratedProduct);
        }

        await _service.ReplaceAsync(product, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}", Name = nameof(DeleteProductAsync))]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        //idempotent - 204 uansett om den eksisterer eller ikke 
        if(await _service.ExistAsync(id, cancellationToken));
            await _service.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    //Flyttes senere
    private GetProductListResponseDto GeneratePageLinks(UrlQueryPagingBaseDto queryParameters, GetProductListResponseDto response)
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
