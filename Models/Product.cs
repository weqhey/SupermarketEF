using System;
using System.Collections.Generic;

namespace SupermarketEF.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string ProductType { get; set; } = null!;

    public List<ProductInReceipt> ProductInReceipts { get; } = new List<ProductInReceipt>();

    public List<ProductInSupermarket> ProductInSupermarkets { get; } = new List<ProductInSupermarket>();
}
