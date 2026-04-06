using Application.Service;
using Domain.Entity;
using UI.Handler;

namespace UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                        new CustomerConsoleHander().Run(); 
                        
                        break;
                    case 2:
                        new StaffConsoleHander().Run();
                        
                        break;
                    case 3:
                        new ProductConsoleHander().Run();
                        break;
                    case 4:
                        new OrderConsoleHander().Run();
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
