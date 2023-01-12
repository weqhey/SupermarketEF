using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketEF.Models;

public partial class ProductInSupermarket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Supermarket { get; set; }
    public int ProductId { get; set; }
    public int Amount { get; set; }
    public DateTime ExpirationDate { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual Supermarket SupermarketNavigation { get; set; } = null!;
}
