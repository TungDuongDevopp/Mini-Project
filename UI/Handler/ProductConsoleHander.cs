using Application.Service;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;
using UI.Helper;

namespace UI.Handler;

internal class ProductConsoleHander: IConsoleHandler<Product>
{   
    private readonly ProductService _productService;
    public ProductConsoleHander(ProductService productService)
    {
        _productService = productService;
    }

    public Product Input()
    {
       
        int id = InputHelper.Input("Enter Product ID:", InputHelper.Parsers.Int, x => x > 0);
        string name = InputHelper.Input("Enter Product Name:", InputHelper.Parsers.String);
        string description = InputHelper.Input("Enter Product Description:", InputHelper.Parsers.String);
        decimal price = InputHelper.Input("Enter Product Price:", InputHelper.Parsers.Decimal, Validator.IsValidMoney);
        int stock = InputHelper.Input("Enter Product Stock Quantity:", InputHelper.Parsers.Int, x => x >= 0);
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
        Console.WriteLine($"{entity.ProductId,-3} | {entity.Name,-25} | {entity.Price.ToString("N0"),-10} | {entity.Description,-30} | {entity.StockQuantity,-5}");
    }

    public void OutputList(IEnumerable<Product> list)
    {
        Console.WriteLine("Danh sách sản phẩm là: ");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"{"ID",-3} | {"Name",-25} | {"Price",-10} | {"Description",-30} | {"StockQuantity",-5}");
        foreach (Product product in list) { 
        Output(product);
        }
    }
    public void Run()
    {

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
                    _productService.Create(Input());
                    break;

                case 2:
                    OutputList(_productService.GetAll());
                    break;

                case 3:
                    Console.Write("Nhập ID cần update: ");
                    int updateId = int.Parse(Console.ReadLine());

                    var updated = Input();
                    
                        _productService.Update(updated);
                    break;

                case 4:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    _productService.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}
