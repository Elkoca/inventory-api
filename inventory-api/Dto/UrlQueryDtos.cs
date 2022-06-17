using Microsoft.AspNetCore.Mvc;

namespace inventory_api.Dto;

public abstract record UrlQueryBaseDto(int Limit = 50, int Page = 1);
public record UrlQueryProductListDto(string? Name) : UrlQueryBaseDto;


//public record UrlQueryParameters(int Limit = 50, int Page = 1);

//public abstract record UrlQueryBaseDto
//{
//    [FromQuery] string Limit { get; set; }
//    [FromQuery] string Page { get; set; }
//}
