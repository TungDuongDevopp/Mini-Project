using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace UI.Handler;

internal class ProductConsoleHander: IConsoleHandler<Product>
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
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
        Console.WriteLine($"{entity.ProductId} - {entity.Name} - {entity.Price} - {entity.Description} - {entity.StockQuantity}");
    }

    public void OutputList(IEnumerable<Product> list)
    {
        Console.WriteLine("Danh sách sản phẩm là: ");
        foreach (Product product in list) { 
        Output(product);
        }
    }
    public void Run()
    {

        var filepath = Path.Combine(BasePath, "File", "Product.json");
        var dataStore = new JsonFileDataStore<Product>(filepath);
        var productrrepo = new ProductRepository(dataStore);

        while (true)
        {
            Console.WriteLine(@"
--- PRODUCT ---
1. Create
2. View All
3. Update
4. Delete
0. Back");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Nhập sai!");
                continue;
            }

            switch (choice)
            {
                case 1:
                    productrrepo.Create(Input());
                    break;

                case 2:
                    OutputList(productrrepo.GetAll());
                    break;

                case 3:
                    Console.Write("Nhập ID cần update: ");
                    int updateId = int.Parse(Console.ReadLine());

                    var updated = Input();
                    
                        productrrepo.Update(updated);
                    break;

                case 4:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    productrrepo.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}
