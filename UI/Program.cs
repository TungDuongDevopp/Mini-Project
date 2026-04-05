using Application.Service;
using Domain.Entity;
using UI.Handler;

namespace UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var orderconsole  =  new OrderConsoleHander() ;
            
            var productService = new ProductService();
            var customerService = new CustomerService();
            productService.Create(new Product() { ProductId =1,Name="Coca",Description="Nước ngọt",Price=15m,StockQuantity=120});
            customerService.Create(new Customer() { CustomerId = 1, Name = "John Doe", Email = "abc", PhoneNumber = "123456789" });
            var orderService = new OrderService(productService,customerService);
            var(customerid,items) = orderconsole.Input();

            orderService.Create(customerid, items);

            orderconsole.OutputList(orderService.GetAll());







        }
    }
}
