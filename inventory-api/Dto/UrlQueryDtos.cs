using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace inventory_api.Dto;

public abstract record UrlQueryPagingBaseDto()
{
    [Range(1, Double.MaxValue)]
    public int Limit { get; init; } = 25;

    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;
};

// Product blir her da her for:  title, name, og articleno
public record UrlQueryGetProductListDto(string? SortBy, string? Search) : UrlQueryPagingBaseDto;
public record UrlQueryGetProductTypeListDto(string? SortBy) : UrlQueryPagingBaseDto;
public record UrlQueryGetVendorListDto(string? SortBy) : UrlQueryPagingBaseDto;

//Trenger nokk ikke disse. De kan like så greit bar bli implementert direkte i controlleren.
//public record UrlQueryGetProductDto([FromRoute] Guid ProductId);
//public record UrlQueryPostProductDto([FromBody] GetProductResponseDto Product);
//public record UrlQueryPutProductDto([FromRoute] Guid ProductId);


//public record UrlQueryParameters(int Limit = 50, int Page = 1);

//public abstract record UrlQueryBaseDto
//{
//    [FromQuery] string Limit { get; set; }
//    [FromQuery] string Page { get; set; }
//}
