using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketEF.Models;

public partial class Supermarket
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Adress { get; set; } = null!;
    public string? Phone { get; set; }
    public List<Department> Departments { get; } = new List<Department>();
    public List<ProductInSupermarket> ProductInSupermarkets { get; } = new List<ProductInSupermarket>();
}
