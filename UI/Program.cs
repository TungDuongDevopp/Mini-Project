
using UI.Factory;
using UI.Handler;

namespace UI
{
    internal class Program
    {
        static void Main(string[] args)

        {
   
           while (true)

            {
                Console.WriteLine("Mời bạn chọn cách thao tác với hệ thống:");
                Console.WriteLine(@"
1.  Thao tác với file
2.  Thao tác với database
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
