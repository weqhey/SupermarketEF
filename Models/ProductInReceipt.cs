using System;
using System.Collections.Generic;

namespace SupermarketEF.Models;

public partial class ProductInReceipt
{
    public int ReceiptId { get; set; }

    public int ProductId { get; set; }

    public int Amount { get; set; }

    public Product Product { get; set; } = null!;

    public Receipt Receipt { get; set; } = null!;
}
