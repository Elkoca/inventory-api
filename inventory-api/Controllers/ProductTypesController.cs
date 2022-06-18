using AutoMapper;
using inventory_api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

}
