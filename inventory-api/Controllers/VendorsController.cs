using AutoMapper;
using inventory_api.Dto;
using inventory_api.Extensions;
using inventory_api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace inventory_api.Controllers;

//[Route("api/Vendors/{VendorId}/[controller]")]
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

    [HttpGet("{VendorId}", Name = nameof(GetVendorAsync))]
    [ProducesResponseType(typeof(GetVendorResponseDto), Status200OK)]
    [ProducesResponseType(Status404NotFound)]
    public async Task<IActionResult> GetVendorAsync([FromRoute] Guid VendorId, CancellationToken cancellationToken)
    {
        var product = await _service.GetByIdAsync(VendorId, cancellationToken);

        if (product == null)
            return NotFound($"product with id {VendorId}, was not found");

        return Ok(product);
    }

    [HttpPost(Name = nameof(PostVendorAsync))]
    [ProducesResponseType(typeof(GetVendorResponseDto), Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PostVendorAsync([FromBody] PostVendorBodyDto newVendor, CancellationToken cancellationToken)
    {
        GetVendorResponseDto createdVendor = await _service.CreateAsync(newVendor, cancellationToken);
        return CreatedAtAction("GetVendor", new { VendorId = createdVendor.VendorId }, createdVendor);
    }

    [HttpPut("{VendorId}", Name = nameof(PutVendorAsync))]
    [ProducesResponseType(typeof(GetVendorResponseDto), Status201Created)]
    [ProducesResponseType(Status204NoContent)]
    [ProducesResponseType(Status400BadRequest)]
    public async Task<IActionResult> PutVendorAsync([FromRoute] Guid VendorId, [FromBody] PutVendorBodyDto product, CancellationToken cancellationToken)
    {
        if (VendorId != product.VendorId)
        {
            return BadRequest("Route, and body id is not matching");
        }

        if (!await _service.ExistAsync(VendorId, cancellationToken))
        {
            var newVendor = _mapper.Map<PostVendorBodyDto>(product);
            GetVendorResponseDto createdVendor = await _service.CreateWithIdAsync(VendorId, newVendor, cancellationToken);
            return CreatedAtAction("GetVendor", new { VendorId = createdVendor.VendorId }, createdVendor);
        }

        await _service.ReplaceAsync(product, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{VendorId}", Name = nameof(DeleteVendorAsync))]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteVendorAsync([FromRoute] Guid VendorId, CancellationToken cancellationToken)
    {
        //idempotent - 204 uansett om den eksisterer eller ikke 
        if (await _service.ExistAsync(VendorId, cancellationToken))
            await _service.DeleteAsync(VendorId, cancellationToken);

        return NoContent();
    }

    //Flyttes senere
    private GetVendorListResponseDto GeneratePageLinks(UrlQueryPagingBaseDto queryParameters, GetVendorListResponseDto response)
    {
        var usedQueryParams = new UsedQueryParameters();
        usedQueryParams.Limit = queryParameters.Limit;
        usedQueryParams.SortBy = queryParameters.SortBy;

        if (response.TotalPages > 1)
        {
            //First
            usedQueryParams.Page = 1;
            var firstRoute = Url.RouteUrl(nameof(GetVendorListAsync), usedQueryParams);
            if (firstRoute != null)
                response.AddResourceLink(LinkedResourceType.First, firstRoute);

            //Last
            usedQueryParams.Page = response.TotalPages;
            var lastRoute = Url.RouteUrl(nameof(GetVendorListAsync), usedQueryParams);
            if (lastRoute != null)
                response.AddResourceLink(LinkedResourceType.Last, lastRoute);
        }

        //Prev (If exist)
        if (response.CurrentPage > 1)
        {
            usedQueryParams.Page = queryParameters.Page - 1;
            var prevRoute = Url.RouteUrl(nameof(GetVendorListAsync), usedQueryParams);
            if (prevRoute != null)
                response.AddResourceLink(LinkedResourceType.Prev, prevRoute);

        }

        //next (If exist (current page is not last))
        if (response.CurrentPage < response.TotalPages)
        {
            usedQueryParams.Page = queryParameters.Page + 1;
            var nextRoute = Url.RouteUrl(nameof(GetVendorListAsync), usedQueryParams);
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
