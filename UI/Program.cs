
using Infrastructure.Db_Context;
using UI.Factory;
using UI.Handler;

namespace UI
{
    internal class Program
    {
        static void Main(string[] args)

        {
            string conn = @"Server=DUONGDG\SQLEXPRESS;Database=ShopMobileDB;Trusted_Connection=True;TrustServerCertificate=True;";
            Console.WriteLine("Chào mừng bạn đến vs phần mềm của dương");
           while (true)

            {
                Console.WriteLine("Mời bạn chọn cách thao tác với hệ thống:");
                Console.WriteLine(@"
1.  Thao tác với file
2.  Thao tác với database
3.  Tạo database
4.  Xóa database
0.  Thoát");
                if (!int.TryParse(Console.ReadLine(), out int choices))
                {
                    Console.WriteLine("Nhập sai!");
                    continue;
                }

                switch (choices)
                {
                    case 1:
                        while (true)
                        {

                            Console.WriteLine(@"
1. Customer
2. Staff
3. Product
4. Order
0. Exit");

                            if (!int.TryParse(Console.ReadLine(), out int choice))
                            {
                                Console.WriteLine("Nhập sai!");
                                continue;
                            }

                            switch (choice)
                            {
                                case 1:
                                    new CustomerConsoleHander(AppFactoryFile.CustomerService).Run();

                                    break;
                                case 2:
                                    new StaffConsoleHander(AppFactoryFile.StaffService).Run();

                                    break;
                                case 3:
                                    new ProductConsoleHander(AppFactoryFile.ProductService).Run();
                                    break;
                                case 4:
                                    new OrderConsoleHanderFile(AppFactoryFile.OrderService).Run();
                                    break;

                                case 0:
                                    return;
                                default:
                                    Console.WriteLine("Không hợp lệ");
                                    break;
                            }
                        }
                       
                    case 2:
                        while (true)
                        {

                            Console.WriteLine(@"
1. Customer
2. Staff
3. Product
4. Order
0. Exit");

                            if (!int.TryParse(Console.ReadLine(), out int choice))
                            {
                                Console.WriteLine("Nhập sai!");
                                continue;
                            }

                            switch (choice)
                            {
                                case 1:
                                    new CustomerConsoleHander(AppFactoryDb.CustomerService).Run();

                                    break;
                                case 2:
                                    new StaffConsoleHander(AppFactoryDb.StaffService).Run();    
                                    break;
                                case 3:
                                    new ProductConsoleHander(AppFactoryDb.ProductService).Run();
                                    break;
                                case 4:
                                    new OrderConsoleHanderDb(AppFactoryDb.OrderService).Run();
                                    break;

                                case 0:
                                    return;
                                default:
                                    Console.WriteLine("Không hợp lệ");
                                    break;
                            }
                        }
         
                    case 3:
                        try
                        {
                            new ShopDbContext(conn).CreateDatabase(new ShopDbContext(conn));
                            Console.WriteLine("Tạo database thành công");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi khi tạo database: {ex.Message}");
                        }
                        break;
                    case 4:
                        try
                        {
                            new ShopDbContext(conn).DeleteDatabase(new ShopDbContext(conn));
                            Console.WriteLine("Xóa database thành công");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi khi xóa database: {ex.Message}");
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
