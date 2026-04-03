namespace Domain.Entity;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public List<OrderDetail> Details { get; set; } = new();
    public decimal TotalAmount { get; set; }
}
