
using Application.Service;
using Infrastructure.Dependency_Injection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UI.Handler;
namespace UI;

internal class Program
{
    static void Main(string[] args)

    {
        //Tạo configuration để đọc file appsettings.json
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        //Tạo DI container
        var services = new ServiceCollection();

        //Đăng ký các service configuration vào DI container
        services.AddSingleton<IConfiguration>(config);

        //Đăng ký repo vào DI container
        services.AddRepository();

        //Đăng ký các service khác vào DI container
       services.AddService();

        //Build service provider
        var serviceProvider = services.BuildServiceProvider();

        //Lấy instance 
        var customerService = serviceProvider.GetRequiredService<CustomerService>();
        var productService = serviceProvider.GetRequiredService<ProductService>();
        var orderService = serviceProvider.GetRequiredService<OrderService>();
        var staffService = serviceProvider.GetRequiredService<StaffService>();
        Console.WriteLine("Chào mừng bạn đến với hệ thống quản lý bán hàng!");
        
        while (true)
        {
            Console.WriteLine("Mời bạn chọn một chức năng:" +
                "1. Quản lý khách hàng" +
                "2. Quản lý sản phẩm" +
                "3. Quản lý đơn hàng" +
                "4. Quản lý nhân viên" +
                "0. Thoát");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                   new CustomerConsoleHander(customerService).Run();
                    break;
                case "2":
                  new ProductConsoleHander(productService).Run();
                    break;
                case "3":
                    new OrderConsoleHander(orderService,productService).Run();
                    break;
                case "4":
                    new StaffConsoleHander(staffService).Run();
                    break;
                case "0":
                    Console.WriteLine("Cảm ơn bạn đã sử dụng hệ thống. Hẹn gặp lại!");
                    return;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                    break;
            }
        }

    }
}
