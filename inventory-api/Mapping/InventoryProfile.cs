using AutoMapper;
using inventory_api.Dto;
using inventory_api.Data.Entities;
using inventory_api.Models;

namespace inventory_api.Mapping;

public class InventoryProfile : Profile
{
    public InventoryProfile()
    {
        //Models -> GetDto
        CreateMap<Product, GetProductResponseDto>();
        CreateMap<Price, GetPriceResponseDto>();
        CreateMap<ProductType, GetProductTypeResponseDto>();
        CreateMap<Vendor, GetVendorResponseDto>();

        //PostBodyDto -> Models
        CreateMap<PostProductBodyDto, Product>();
        CreateMap<PostPriceBodyDto, Price>();
        CreateMap<PostProductTypeBodyDto, ProductType>();
        CreateMap<PostVendorBodyDto, Vendor>();

        //PutBodyDto -> Models
        CreateMap<PutProductBodyDto, Product>();
        CreateMap<PutPriceBodyDto, Price>();
        CreateMap<PutProductTypeBodyDto, ProductType>();
        CreateMap<PutVendorBodyDto, Vendor>();

        //PutBodyDto -> PostBodyDto
        CreateMap<PutProductBodyDto, PostProductBodyDto>();
        CreateMap<PutPriceBodyDto, PostPriceBodyDto>();

        CreateMap<PutVendorBodyDto, PostVendorBodyDto>();
        CreateMap<PutProductTypeBodyDto, PostProductTypeBodyDto>();

        //Other
        CreateMap<PagedModel<Product>, GetProductListResponseDto>();
        CreateMap<PagedModel<ProductType>, GetProductTypeListResponseDto>();
        CreateMap<PagedModel<Vendor>, GetVendorListResponseDto>();


        //Tror kanskje ikke jeg trenger denne:
        //.ForMember( 
        //    dest => dest.Price.ProductId,
        //    opt => opt.MapFrom(src => src.productId)
        //)
    }
}
