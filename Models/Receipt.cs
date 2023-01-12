using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketEF.Models;

public partial class Receipt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int WorkerId { get; set; }
    public decimal Price { get; set; }
    public virtual List<ProductInReceipt> ProductInReceipts { get; } = new List<ProductInReceipt>();
    public virtual Worker Worker { get; set; } = null!;
}
