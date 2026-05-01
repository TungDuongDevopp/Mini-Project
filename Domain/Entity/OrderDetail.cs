using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;
[Table("OrderDetail")]
public class OrderDetail
{
    [Key]   
    public int OrderDetailId { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }


    public int Quantity { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }
}
