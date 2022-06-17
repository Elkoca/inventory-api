using Microsoft.AspNetCore.Mvc;

namespace inventory_api.Dto;

public abstract record UrlQueryPagingBaseDto(int Limit = 50, int Page = 1);
public record UrlQueryGetProductListDto(string? SortBy, string? Name, string? Id, string? Product) : UrlQueryPagingBaseDto;

//Trenger nokk ikke disse. De kan like så greit bar bli implementert direkte i controlleren.
//public record UrlQueryGetProductDto([FromRoute] Guid Id);
//public record UrlQueryPostProductDto([FromBody] GetProductResponseDto Product);
//public record UrlQueryPutProductDto([FromRoute] Guid Id);


//public record UrlQueryParameters(int Limit = 50, int Page = 1);

//public abstract record UrlQueryBaseDto
//{
//    [FromQuery] string Limit { get; set; }
//    [FromQuery] string Page { get; set; }
//}
