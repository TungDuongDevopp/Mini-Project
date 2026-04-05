using Domain.Entity;

namespace UI.Handler;
internal class OrderConsoleHander 

{
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
        foreach (var item in list)
        {
            Output(item);
        }
    }
}