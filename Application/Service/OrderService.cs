using Application.Interface;
using Domain.Entity;

namespace Application.Service;

public class OrderService : IOrder
{
    private readonly IBaseRepository<Product> _productService;
    private readonly IBaseRepository<Customer> _customerService;
    private List<Order> _orders= new();

    public OrderService(IBaseRepository<Product> productService, IBaseRepository<Customer> customerService)
    {
        _productService = productService;
        _customerService = customerService;
    }
    public void Create(int customerId, List<(int productId, int quantity)> items)
    {
        //Kiểm tra sự tồn tại của customer
        var existingCustomer = _customerService.GetById(customerId);
        if (existingCustomer == null)
        {
            throw new Exception($"Customer with ID {customerId} not found.");
        }
        // Tạo đơn hàng mới

        var order = new Order
        {
            OrderId = _orders.Count + 1,
            CustomerId = customerId,
            TotalAmount = 0
        };

        decimal total = 0;
        //Duyệt tưng từng sản phẩm trong đơn hàng, kiểm tra sự tồn tại của sản phẩm và tính tổng tiền
        foreach (var item in items)
        {
            // Kiểm tra sự tồn tại của sản phẩm
            var product = _productService.GetById(item.productId);
            if (product == null)
            {
                throw new Exception($"Product with ID {item.productId} not found.");
            }
            // Kiểm tra số lượng hợp lệ
            if (item.quantity < 0)
            {
                throw new Exception($"Quantity for product ID {item.productId} cannot be negative. Requested: {item.quantity}");
            }
            //Kiểm tra số lượng tồn kho
            if (product.StockQuantity < item.quantity)
            {
                throw new Exception($"Not enough stock for product ID {item.productId}. Available: {product.StockQuantity}, Requested: {item.quantity}");
            }
            //Cập nhật số lượng kho
            product.StockQuantity -= item.quantity;
            // Tạo chi tiết đơn hàng và tính tổng tiền
            var orderDetail = new OrderDetail

            {   OrderDetailId = order.Details.Count + 1,
                OrderId = order.OrderId,
                ProductId = item.productId,
                Quantity = item.quantity,
                Price = product.Price
            };
            // Thêm chi tiết đơn hàng vào đơn hàng
            order.Details.Add(orderDetail);
            // Tính tổng tiền
            total += orderDetail.Quantity * orderDetail.Price;
        }
        // Cập nhật tổng tiền cho đơn hàng
        order.TotalAmount = total;
        // Lưu đơn hàng vào danh sách
        _orders.Add(order);
    }

    public bool Delete(int id)
    {
        var existingOrder = GetById(id);
        if (existingOrder == null)
        {
            return false;
        }
        _orders.Remove(existingOrder);

        return true;
    }

    public IReadOnlyList<Order> GetAll()
    {
        return _orders;
    }

    public Order? GetById(int id)
    {
       var order = _orders.FirstOrDefault(o => o.OrderId == id);
         return order;
    }


}
