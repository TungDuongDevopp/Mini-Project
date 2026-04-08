using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;


namespace UI.Handler;
internal class OrderConsoleHander 

{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    public (int customerId, List<(int productId, int quantity)> items) Input()
    {
        Console.Write("Enter CustomerId: ");
        int customerId = int.Parse(Console.ReadLine()!);

        var items = new List<(int productId, int quantity)>();

        while (true)
        {
            Console.Write("Enter ProductId (0 to stop): ");
            int productId = int.Parse(Console.ReadLine()!);

            if (productId == 0)
                break;

            Console.Write("Enter Quantity: ");
            int quantity = int.Parse(Console.ReadLine()!);

            items.Add((productId, quantity));
        }

        return (customerId, items);
    }
    public void Output(Order entity)
    {
        Console.WriteLine($"{entity.OrderId} - {entity.CustomerId} - {entity.TotalAmount}");
        foreach (var detail in entity.Details)
        {
            Console.WriteLine($"{detail.OrderDetailId} - {detail.ProductId} - {detail.Quantity} - {detail.Price}");
        }
    }

    public void OutputList(IEnumerable<Order> list)
    {
        Console.WriteLine("Danh sách đơn đặt hàng là:");
        foreach (var item in list)
        {
            Output(item);
        }
    }

    public void Run()
    {
        

     var ordersevice = AppFactory.CreateOrderService();
        var filePath = Path.Combine(BasePath, "File", "Order.json");
        var datastore = new JsonFileDataStore<Order>(filePath);
        var orderRepo =AppFactory.CreateOrderRepository();

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
                    ordersevice.Create(customerId, items);
                    break;

                case 2:
                    OutputList(orderRepo.GetAll());
                    break;
                case 3:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    orderRepo.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}