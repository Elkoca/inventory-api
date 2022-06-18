using AutoMapper;
using inventory_api.Dto;
using inventory_api.Data.Entities;
using inventory_api.Models;

namespace inventory_api.Mapping;

public class InventoryProfile : Profile
{
    public InventoryProfile()
    {
        //Models -> Dto
        CreateMap<Product, GetProductResponseDto>();
        CreateMap<Currency, GetCurrencyResponseDto>();
        CreateMap<ProductType, GetProductTypeResponseDto>();
        CreateMap<Vendor, GetVendorResponseDto>();

        //Other
        CreateMap<PagedModel<Product>, GetProductListResponseDto>();
    }
}
