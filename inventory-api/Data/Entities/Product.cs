using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_api.Data.Entities;

public class Product : DbBase
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
}

//public class Products
//{
//    public int CurrentPage { get; set; }

//    public int TotalItems { get; set; }

//    public int TotalPages { get; set; }

//    public List<Product> Items { get; set; }
//}