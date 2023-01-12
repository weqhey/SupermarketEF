using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketEF.Models;

public partial class ProductInReceipt
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReceiptId { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual Receipt Receipt { get; set; } = null!;
}
