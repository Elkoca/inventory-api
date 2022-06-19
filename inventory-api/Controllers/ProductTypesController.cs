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
public class ProductTypesController : ControllerBase
{
    private readonly IProductTypeService _service;
    public readonly IMapper _mapper;

    public ProductTypesController(IProductTypeService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    [HttpGet(Name = nameof(GetProductTypeListAsync))]
    [ProducesResponseType(typeof(GetProductTypeListResponseDto), Status200OK)]
    public async Task<IActionResult> GetProductTypeListAsync([FromQuery] UrlQueryGetProductTypeListDto urlQueryParameters, CancellationToken cancellationToken)
    {
        var productTypes = await _service.GetByPageAsync(
                            urlQueryParameters.Limit,
                            urlQueryParameters.Page,
                            urlQueryParameters.SortBy,
                            cancellationToken);

        return Ok(GeneratePageLinks(urlQueryParameters, productTypes));
    }

    [HttpGet("{ProductTypeId}", Name = nameof(GetProductTypeAsync))]
    [ProducesResponseType(typeof(GetProductTypeResponseDto), Status200OK)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> GetProductTypeAsync([FromRoute] Guid ProductTypeId, CancellationToken cancellationToken)
    {
        var product = await _service.GetByIdAsync(ProductTypeId, cancellationToken);

        if (product == null)
            return NotFound($"product with id {ProductTypeId}, was not found");

        return Ok(product);
    }

    [HttpPost(Name = nameof(PostProductTypeAsync))]
    [ProducesResponseType(typeof(GetProductTypeResponseDto), Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PostProductTypeAsync([FromBody] PostProductTypeBodyDto newProductType, CancellationToken cancellationToken)
    {
        GetProductTypeResponseDto createdProductType = await _service.CreateAsync(newProductType, cancellationToken);
        return CreatedAtAction("GetProductType", new { ProductTypeId = createdProductType.ProductTypeId }, createdProductType);
    }

    [HttpPut("{ProductTypeId}", Name = nameof(PutProductTypeAsync))]
    [ProducesResponseType(typeof(GetProductTypeResponseDto), Status201Created)]
    [ProducesResponseType(Status204NoContent)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PutProductTypeAsync([FromRoute] Guid ProductTypeId, [FromBody] PutProductTypeBodyDto product, CancellationToken cancellationToken)
    {
        if (ProductTypeId != product.ProductTypeId)
        {
            return BadRequest("Route, and body id is not matching");
        }

        if (!await _service.ExistAsync(ProductTypeId, cancellationToken))
        {
            var newProductType = _mapper.Map<PostProductTypeBodyDto>(product);
            GetProductTypeResponseDto createdProductType = await _service.CreateWithIdAsync(ProductTypeId, newProductType, cancellationToken);
            return CreatedAtAction("GetProductType", new { ProductTypeId = createdProductType.ProductTypeId }, createdProductType);
        }

        await _service.ReplaceAsync(product, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{ProductTypeId}", Name = nameof(DeleteProductTypeAsync))]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteProductTypeAsync([FromRoute] Guid ProductTypeId, CancellationToken cancellationToken)
    {
        //idempotent - 204 uansett om den eksisterer eller ikke 
        if (await _service.ExistAsync(ProductTypeId, cancellationToken))
            await _service.DeleteAsync(ProductTypeId, cancellationToken);

        return NoContent();
    }

    private GetProductTypeListResponseDto GeneratePageLinks(UrlQueryPagingBaseDto queryParameters, GetProductTypeListResponseDto response)
    {
        var usedQueryParams = new UsedQueryParameters();
        usedQueryParams.Limit = queryParameters.Limit;
        usedQueryParams.SortBy = queryParameters.SortBy;

        if (response.TotalPages > 1)
        {
            //First
            usedQueryParams.Page = 1;
            var firstRoute = Url.RouteUrl(nameof(GetProductTypeListAsync), usedQueryParams);
            if (firstRoute != null)
                response.AddResourceLink(LinkedResourceType.First, firstRoute);

            //Last
            usedQueryParams.Page = response.TotalPages;
            var lastRoute = Url.RouteUrl(nameof(GetProductTypeListAsync), usedQueryParams);
            if (lastRoute != null)
                response.AddResourceLink(LinkedResourceType.Last, lastRoute);
        }

        //Prev (If exist)
        if (response.CurrentPage > 1)
        {
            usedQueryParams.Page = queryParameters.Page - 1;
            var prevRoute = Url.RouteUrl(nameof(GetProductTypeListAsync), usedQueryParams);
            if (prevRoute != null)
                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

        }

        //next (If exist (current page is not last))
        if (response.CurrentPage < response.TotalPages)
        {
            usedQueryParams.Page = queryParameters.Page + 1;
            var nextRoute = Url.RouteUrl(nameof(GetProductTypeListAsync), usedQueryParams);
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
    }
}
