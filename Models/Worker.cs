using System;
using System.Collections.Generic;

namespace SupermarketEF.Models;

public partial class Worker
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Birthday { get; set; }
    public string Position { get; set; } = null!;
    public DateTime DateOfEmployment { get; set; }
    public List<Receipt> Receipts { get; } = new List<Receipt>();
    public List<Department> Departments { get; } = new List<Department>();
}
