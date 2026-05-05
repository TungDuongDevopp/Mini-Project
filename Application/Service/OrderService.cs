using Application.Interface;
using Domain.Entity;



public class OrderService : IOrder
{
    private readonly IBaseRepository<Order> _orderRepo;
    private readonly IBaseRepository<Customer> _customerRepo;
    private readonly IProductRepository _productRepo;

    public OrderService(
        IBaseRepository<Order> orderRepo,
        IBaseRepository<Customer> customerRepo,
        IProductRepository productRepo)
    {
        _orderRepo = orderRepo;
        _customerRepo = customerRepo;
        _productRepo = productRepo;
    }

    public void Create(int customerId, List<(int productId, int quantity)> items)
    {
        var customer = _customerRepo.GetById(customerId);
        if (customer == null)
            throw new Exception("Customer not found");

        if (items == null || !items.Any())
            throw new Exception("Order must contain at least one product");

        var order = new Order
        {
            CustomerId = customerId,
            Details = new List<OrderDetail>()
        };

        decimal total = 0;

        foreach (var item in items)
        {
            var product = _productRepo.GetById(item.productId);

            if (product == null)
                throw new Exception("Product not found");

            if (product.StockQuantity < item.quantity)
                throw new Exception("Not enough stock");

            var detail = new OrderDetail
            {
                ProductId = item.productId,
                Quantity = item.quantity,
                Price = product.Price
            };

            order.Details.Add(detail);
            total += detail.Quantity * detail.Price;
        }

        order.TotalAmount = total;

        _orderRepo.Create(order);
    }

    public bool Delete(int id)
    {
        _orderRepo.Delete(id);
         return true;
    }

    public IReadOnlyList<Order> GetAll()
    => _orderRepo.GetAll().ToList();
    

    public Order? GetById(int id)
       => _orderRepo.GetById(id);
    
}
