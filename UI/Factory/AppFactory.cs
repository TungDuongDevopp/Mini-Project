using Application.Service;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;

internal static class AppFactory
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

    // =========================
    // DATASTORE (Singleton)
    // =========================

    private static JsonFileDataStore<Customer>? _customerStore;
    private static JsonFileDataStore<Product>? _productStore;
    private static JsonFileDataStore<Order>? _orderStore;

    private static JsonFileDataStore<Customer> CustomerStore =>
        _customerStore ??= new JsonFileDataStore<Customer>(
            Path.Combine(BasePath, "File", "Customer.json"));

    private static JsonFileDataStore<Product> ProductStore =>
        _productStore ??= new JsonFileDataStore<Product>(
            Path.Combine(BasePath, "File", "Product.json"));

    private static JsonFileDataStore<Order> OrderStore =>
        _orderStore ??= new JsonFileDataStore<Order>(
            Path.Combine(BasePath, "File", "Order.json"));

    // =========================
    // REPOSITORY (Singleton)
    // =========================

    private static CustomerRepository? _customerRepo;
    private static ProductRepository? _productRepo;
    private static OrderRepository? _orderRepo;

    public static CustomerRepository CustomerRepository =>
        _customerRepo ??= new CustomerRepository(CustomerStore);

    public static ProductRepository ProductRepository =>
        _productRepo ??= new ProductRepository(ProductStore);

    public static OrderRepository OrderRepository =>
        _orderRepo ??= new OrderRepository(OrderStore);

    // =========================
    // SERVICE (Singleton)
    // =========================

    private static CustomerService? _customerService;
    private static ProductService? _productService;
    private static OrderService? _orderService;

    public static CustomerService CustomerService =>
        _customerService ??= new CustomerService(CustomerRepository);

    public static ProductService ProductService =>
        _productService ??= new ProductService(ProductRepository);

    public static OrderService OrderService =>
        _orderService ??= new OrderService(
            OrderRepository,
            ProductRepository,
            CustomerService
        );
}