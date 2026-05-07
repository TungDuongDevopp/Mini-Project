using Application.Interface;
using Application.Service;
using Domain.Entity;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Dependency_Injection;

public static class DependencyInjection
{
    public static IServiceCollection AddRepository(
   this IServiceCollection services)
    {
        services.AddSingleton<IBaseRepository<Customer>, CustomerRepositoryDbContext>();
        services.AddSingleton<IProductRepository, ProductRepositoryDbContext>();
        services.AddSingleton<IBaseRepository<Order>, OrderRepositoryDbContext>();
        services.AddSingleton<IBaseRepository<Staff>, StaffRepositoryDbContext>();
        return services;
    }

    public static IServiceCollection AddService(this IServiceCollection services)
    {
        services.AddSingleton<CustomerService>();
        services.AddSingleton<ProductService>();
        services.AddSingleton<OrderService>();
        services.AddSingleton<StaffService>();
        return services;
    }
}
