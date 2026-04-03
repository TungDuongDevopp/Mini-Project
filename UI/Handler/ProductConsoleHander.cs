using Domain.Entity;

namespace UI.Handler;

internal class ProductConsoleHander: IConsoleHandler<Product>
{
    public Product Input()
    {
        Console.WriteLine("Enter Product Id:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Product Name:");
        string name = Console.ReadLine()!;
        Console.WriteLine("Enter Product Description");
        string description = Console.ReadLine()!;
        Console.WriteLine("Enter Product Price:");
        decimal price = decimal.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Product Stockquantity:");
        int stock = int.Parse(Console.ReadLine()!);
        return new Product
        {
            ProductId = id,
            Name = name,
            Price = price,
            Description = description,
            StockQuantity = stock
        };
    }

    public void Output(Product entity)
    {
        Console.WriteLine($"{entity.ProductId} - {entity.Name} -{entity.Price} - {entity.Description} -{entity.StockQuantity}");
    }

    public void OutputList(IEnumerable<Product> list)
    {
        foreach (Product product in list) { 
        Output(product);
        }
    }
}
