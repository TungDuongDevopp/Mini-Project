using Application.Service;
using Domain.Entity;

namespace UI.Handler;
internal class OrderConsoleHander
{
    private readonly OrderService _orderService;
    private readonly ProductService _productService;
    public OrderConsoleHander(OrderService orderService, ProductService productService)
    {
        _orderService = orderService;
        _productService = productService;
    }

    public (int customerId, List<(int productId, int quantity)> items) Input()
    {

        int customerId = InputHelper.Input("Enter Customer ID:", InputHelper.Parsers.Int, x => x > 0);

        var items = new List<(int productId, int quantity)>();

        while (true)
        {

            int productId = InputHelper.Input("Enter Product ID (0 to stop):", InputHelper.Parsers.Int, x => x >= 0);

            if (productId == 0)
                break;
            int quantity = InputHelper.Input("Enter Quantity:", InputHelper.Parsers.Int, x => x > 0);

            items.Add((productId, quantity));
        }

        return (customerId, items);
    }
    public void Output(Order entity)
    { 
        var products = _productService.GetAll().ToDictionary(p => p.ProductId);

        Console.WriteLine($"\nORDER: {entity.OrderId} - Customer: {entity.CustomerId}");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
        // In tiêu đề cột ngay tại đây
        Console.WriteLine($"{"ID",-5} | {"ProductName",-30} | {"Quantity",-8} | {"Price",-10}");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");

        foreach (var item in entity.Details)
        {
            var productName = products.ContainsKey(item.ProductId)
                ? products[item.ProductId].Name
                : "Unknown";

            Console.WriteLine($"{item.ProductId,-5} | {productName,-30} | {item.Quantity,-8} | {item.Price.ToString("N0"),10}");
        }
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"TOTAL: {entity.TotalAmount:N0} VNĐ"); // Thêm :N0 để định dạng số tiền cho đẹp
    }

    public void OutputList(IEnumerable<Order> list)
    {
        Console.WriteLine("DANH SÁCH CÁC ĐƠN HÀNG");
        foreach (var item in list)
        {
            Output(item);
        }
    }


    public void Run()
    {
        while (true)
        {
            Console.WriteLine(@"
--- Order ---
1. Create
2. View All
3. Delete
0. Back");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Nhập sai!");
                continue;
            }

            switch (choice)
            {
                case 1:
                    var (customerId, items) = Input();
                    _orderService.Create(customerId, items);
                    break;

                case 2:
                    OutputList(_orderService.GetAll());
                    break;
                case 3:
                    Console.Write("Nhập ID cần xóa: ");
                    
                    int deleteId = int.Parse(Console.ReadLine() ?? "0");
                    var existingOrder = _orderService.GetById(deleteId);
                    if (existingOrder == null)
                    {
                        Console.WriteLine("Order not found.");
                        break;
                    }
                    _orderService.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}