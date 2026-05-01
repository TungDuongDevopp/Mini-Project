using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;
[Table("Order")]
public class Order
{
    [Key]
    public int OrderId { get; set; }

    [ForeignKey("Customer")]
    public int CustomerId { get; set; }

    public List<OrderDetail> Details { get; set; } = new();

    [Column(TypeName = "money")]
    public decimal TotalAmount { get; set; }
}
