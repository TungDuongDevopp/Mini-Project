using Application.Interface;
using Domain.Entity;

public class OrderServiceFile: IOrder
{
    private readonly IBaseRepository<Order> _orderRepo;
    private readonly IProductRepository _productRepo;
    private readonly IBaseRepository<Customer> _customerRepo;

    public OrderServiceFile(
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

    public bool Delete(int id)
    => _orderRepo.Delete(id);

    public IReadOnlyList<Order> GetAll()
   => _orderRepo.GetAll().ToList();

    public Order? GetById(int id)
    => _orderRepo.GetById(id);
}

public class OrderServiceDb : IOrder
{
    private readonly IOrderRepository _orderRepo;
    private readonly IBaseRepository<Customer> _customerRepo;
    private readonly IProductRepository _productRepo;

    public OrderServiceDb(
        IOrderRepository orderRepo,
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

        _orderRepo.CreateOrderWithDetails(order);
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Order> GetAll()
    {
        throw new NotImplementedException();
    }

    public Order? GetById(int id)
    {
        throw new NotImplementedException();
    }
}
