namespace inventory_api.Data.Entities;

public class Currency : IEntity
{
    public Guid CurrencyId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public string? Code { get; set; } // USD, NOK
    public string? Amount { get; set; } // 4999,99

    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
}

//public class Currency : IEntity
//{
//    public Guid CurrencyId { get; set; }
//    public DateTime Created { get; set; }
//    public DateTime LastUpdated { get; set; }

//    public string? Name { get; set; } // Norske kroner, Amerikanske dollar
//    public string? Code { get; set; } // USD, NOK
//    public string? Symbol { get; set; } // €, $, kr 
//}