using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inventory_api.Data.Entities;

public class Product : IEntity
{
    public Guid ProductId { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }

    public string? Name { get; set; } // Navnet på produktet
    public string? Title { get; set; } // Tittel på produktet (Fra leverandør, eller noe jeg lager)
    public string? Description { get; set; } //Beskrivelse av produktet
    public int Stock { get; set; } //Antal varer
    public int? ArticleNo { get; set; } //Barcode
    public Price? Price { get; set; }
    public Guid? ProductTypeId { get; set; }
    public ProductType? ProductType { get; set; }
    public Guid? VendorId { get; set; }
    public Vendor? Vendor { get; set; }


}