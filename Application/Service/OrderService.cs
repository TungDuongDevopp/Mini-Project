using Application.Interface;
using Domain.Entity;

public class OrderService
{
    private readonly IBaseRepository<Order> _orderRepo;
    private readonly IProductRepository _productRepo;
    private readonly IBaseRepository<Customer> _customerRepo;

    public OrderService(
        IBaseRepository<Order> orderRepo,
        IProductRepository productRepo,
        IBaseRepository<Customer> customerRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
        _customerRepo = customerRepo;
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
            TotalAmount = 0
        };

        decimal total = 0;

        // Phase 1: validate
        foreach (var item in items)
        {
            var product = _productRepo.GetById(item.productId);

            if (product == null)
                throw new Exception($"Product {item.productId} not found");

            if (item.quantity <= 0)
                throw new Exception("Invalid quantity");

            if (product.StockQuantity < item.quantity)
                throw new Exception("Not enough stock");
        }

        // Phase 2: tạo order + trừ stock
        foreach (var item in items)
        {
            var product = _productRepo.GetById(item.productId);

            _productRepo.DecreaseStock(item.productId, item.quantity);

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
}