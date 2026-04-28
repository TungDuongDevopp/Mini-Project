using Application.Interface;
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
       
        
        string name = InputHelper.Input("Enter Product Name:", InputHelper.Parsers.String);
        string description = InputHelper.Input("Enter Product Description:", InputHelper.Parsers.String);
        decimal price = InputHelper.Input("Enter Product Price:", InputHelper.Parsers.Decimal, Validator.IsValidMoney);
        int stock = InputHelper.Input("Enter Product Stock Quantity:", InputHelper.Parsers.Int, x => x >= 0);
        return new Product
        {
         
            Name = name,
            Price = price,
            Description = description,
            StockQuantity = stock
        };
    }

    public void Output(Product entity)
        
    {  
        Console.WriteLine($"{entity.ProductId,-3} | {entity.Name,-35} | {entity.Price.ToString("N0"),-10} | {entity.Description,-40} | {entity.StockQuantity,-5}");
    }

    public void OutputList(IEnumerable<Product> list)
    {
        Console.WriteLine("Danh sách sản phẩm là: ");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"{"ID",-3} | {"Name",-35} | {"Price",-10} | {"Description",-40} | {"StockQuantity",-5}");
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
                    var existing = _productService.GetById(updateId);
                    if(existing != null)
                    {
                        existing.Name = InputHelper.Input("Enter Product Name:", InputHelper.Parsers.String);
                        existing.Description = InputHelper.Input("Enter Product Description:", InputHelper.Parsers.String);
                        existing.Price = InputHelper.Input("Enter Product Price:", InputHelper.Parsers.Decimal, Validator.IsValidMoney);
                        existing.StockQuantity = InputHelper.Input("Enter Product Stock Quantity:", InputHelper.Parsers.Int, x => x >= 0);
                        _productService.Update(existing);
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy sản phẩm với ID đã nhập.");
                    }
                        break;

                case 4:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    if(_productService.GetById(deleteId) == null)
                    {
                        Console.WriteLine("Không tìm thấy sản phẩm với ID đã nhập.");
                        break;
                    }
                    _productService.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}
