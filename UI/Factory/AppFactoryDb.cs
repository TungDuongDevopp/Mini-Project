

using Application.Interface;
using Infrastructure.Repository;

namespace UI.Factory
{
    public class AppFactoryDb
    {
        private static readonly string _connectionString = @"Server=DUONGDG\SQLEXPRESS;Database=ShopDB;Trusted_Connection=True;TrustServerCertificate=True;";
        // =========================
        // REPOSITORY (Singleton)
        // =========================
        private static CustomerRepositoryDb? _customerRepo;
        private static ProductRepositoryDb? _productRepo;
        private static OrderRepositoryDb? _orderRepo;
        private static StaffRepositoryDb? _staffRepo;

        public static CustomerRepositoryDb CustomerRepository =>
            _customerRepo ??= new CustomerRepositoryDb(_connectionString);

        public static ProductRepositoryDb ProductRepository =>
            _productRepo ??= new ProductRepositoryDb(_connectionString);

        public static OrderRepositoryDb OrderRepository =>
            _orderRepo ??= new OrderRepositoryDb(_connectionString);
        public static StaffRepositoryDb StaffRepository =>
            _staffRepo ??= new StaffRepositoryDb(_connectionString);
        // =========================
        // Service (Singleton)
        // =========================
        private static CustomerService? _customerService;
        private static ProductService? _productService;
        private static OrderServiceDb? _orderService;
        private static StaffService? _staffService;

        public static CustomerService CustomerService =>
            _customerService ??= new CustomerService(CustomerRepository);
        public static ProductService ProductService =>
            _productService ??= new ProductService(ProductRepository);
        public static OrderServiceDb OrderService =>
            _orderService ??= new OrderServiceDb(OrderRepository,CustomerRepository, ProductRepository);   
        public static StaffService StaffService =>
            _staffService ??= new StaffService(StaffRepository);
    }
}
