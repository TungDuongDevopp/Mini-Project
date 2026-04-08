using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;

internal class AppFactory
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

    public static OrderService CreateOrderService()
    {
        var orderRepo = CreateOrderRepository();
        var productRepo = CreateProductRepository();
        var customerRepo = CreateCustomerRepository();

        return new OrderService(orderRepo, productRepo, customerRepo);
    }

    public static OrderRepository CreateOrderRepository()
    {
        var path = Path.Combine(BasePath, "File", "Order.json");
        var store = new JsonFileDataStore<Order>(path);
        return new OrderRepository(store);
    }

    private static ProductRepository CreateProductRepository()
    {
        var path = Path.Combine(BasePath, "File", "Product.json");
        var store = new JsonFileDataStore<Product>(path);
        return new ProductRepository(store);
    }

    private static CustomerRepository CreateCustomerRepository()
    {
        var path = Path.Combine(BasePath, "File", "Customer.json");
        var store = new JsonFileDataStore<Customer>(path);
        return new CustomerRepository(store);
    }
}