
using Application.Service;
using Infrastructure.Dependency_Injection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

    }
}
