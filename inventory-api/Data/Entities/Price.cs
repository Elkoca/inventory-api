namespace inventory_api.Data.Entities;

public class Price : IEntity
{
    public Guid PriceId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public string? Code { get; set; } // USD, NOK
    public double Amount { get; set; } // 4999,99

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}

//public class Price : IEntity
//{
//    public Guid PriceId { get; set; }
//    public DateTime Created { get; set; }
//    public DateTime LastUpdated { get; set; }

//    public string? Name { get; set; } // Norske kroner, Amerikanske dollar
//    public string? Code { get; set; } // USD, NOK
//    public string? Symbol { get; set; } // €, $, kr 
//}