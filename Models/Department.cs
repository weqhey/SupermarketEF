using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupermarketEF.Models;

public partial class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string ProductType { get; set; } = null!;
    public int SupermarketId { get; set; }
    public Supermarket Supermarket { get; set; } = null!;
    public List<Worker> Workers { get; } = new List<Worker>();
}
