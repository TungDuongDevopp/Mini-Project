using Application.Interface;
using Domain.Entity;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Dependency_Injection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
   this IServiceCollection services)
    {
        services.AddSingleton<IBaseRepository<Customer>, CustomerRepositoryDb>();
        services.AddSingleton<IProductRepository, ProductRepositoryDb>();
        services.AddSingleton<IBaseRepository<Order>, OrderRepositoryDb>();
        services.AddSingleton<IBaseRepository<Staff>, StaffRepositoryDb>();
        return services;
    }
}
