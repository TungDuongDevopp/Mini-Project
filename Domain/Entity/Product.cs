using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;
[Table("Product")]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public List<OrderDetail> OrderDetails { get; set; } = new();
}
