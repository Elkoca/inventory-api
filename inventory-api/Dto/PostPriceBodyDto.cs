using System.ComponentModel.DataAnnotations;

namespace inventory_api.Dto;

public record PostPriceBodyDto
{
    public string? Code { get; init; } // USD, NOK
    public double? Amount { get; init; } // 4999,99
    //[Required]
    //public Guid ProductId { get; init; }
}
