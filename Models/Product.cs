using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketEF.Models;

public partial class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string ProductType { get; set; } = null!;
    public virtual List<ProductInReceipt> ProductInReceipts { get; } = new List<ProductInReceipt>();
    public virtual List<ProductInSupermarket> ProductInSupermarkets { get; } = new List<ProductInSupermarket>();
}
