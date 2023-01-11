using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketEF.Models;

public partial class Worker : Person
{
    public string Position { get; set; } = null!;
    public DateTime DateOfEmployment { get; set; }
    public List<Receipt> Receipts { get; } = new List<Receipt>();
    public List<Department> Departments { get; } = new List<Department>();
}
