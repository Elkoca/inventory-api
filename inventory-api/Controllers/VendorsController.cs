using AutoMapper;
using inventory_api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

}
