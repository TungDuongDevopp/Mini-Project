 using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using UI.Handler;

namespace UI
{
    internal class Program
    {
        static void Main(string[] args)

        {
            var conectionString = @"Server=DUONGDG\SQLEXPRESS;Database=ShopDB;Trusted_Connection=True;TrustServerCertificate=True;";
            var connection = new SqlDbConnection(conectionString);
            while (true)
            {
                Console.WriteLine(@"
1. Customer
2. Staff
3. Product
4. Order
5.Test database connection
0. Exit");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Nhập sai!");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        new CustomerConsoleHander(AppFactory.CustomerService).Run(); 
                        
                        break;
                    case 2:
                        new StaffConsoleHander(AppFactory.StaffService).Run();
                        
                        break;
                    case 3:
                        new ProductConsoleHander(AppFactory.ProductService).Run();
                        break;
                    case 4:
                        new OrderConsoleHander(AppFactory.OrderService).Run();
                        break;
                    case 5:
                        try
                        {
                            using (var dbConnection = connection.GetConnection())
                            {
                                dbConnection.Open();
                                Console.WriteLine("Kết nối thành công!");
                                dbConnection.Close();
                                Console.WriteLine("Đóng kết nối");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Kết nối thất bại: {ex.Message}");
                        }
                      
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Không hợp lệ");
                        break;
                }
            }



        }
    }
}
