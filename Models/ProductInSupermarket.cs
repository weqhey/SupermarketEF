using System;
using System.Collections.Generic;

namespace SupermarketEF.Models;

public partial class ProductInSupermarket
{
    public int Id { get; set; }

    public int Supermarket { get; set; }

    public int ProductId { get; set; }

    public int Amount { get; set; }

    public DateTime ExpirationDate { get; set; }

    public Product Product { get; set; } = null!;

    public Supermarket SupermarketNavigation { get; set; } = null!;
}
