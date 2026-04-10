using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;
using static InputHelper;

namespace UI.Handler;

internal class CustomerConsoleHander: IConsoleHandler<Customer>
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    public Customer Input()
    {
        int id = InputHelper.Input("Enter Customer Id:", Parsers.Int, x => x > 0);
        string name = InputHelper.Input("Enter Customer Name:", Parsers.String);
        string phoneNumber = InputHelper.Input("Enter Customer Phone Number:", Parsers.String);
        string email = InputHelper.Input("Enter Customer Email:", Parsers.String);
        return new Customer
        {
            CustomerId = id,
            Name = name,
            PhoneNumber = phoneNumber,
            Email = email
        };
    }

    public void Output(Customer entity)
    { 
        Console.WriteLine($"{entity.CustomerId,-3} | {entity.Name,-25} | {entity.Email,-25} | {entity.PhoneNumber,-15}");
    }

    public void OutputList(IEnumerable<Customer> list)
    {
        Console.WriteLine("Danh sách khách hàng là: ");
        Console.WriteLine("----------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"{"ID",-3} | {"Name",-25} | {"Email",-25} | {"PhoneNumber",-15}");
        foreach (var customer in list)
        {
           
           Output(customer);
        }
    }

    public void Run()
       
    {
        var filepath = Path.Combine(BasePath, "File", "Customer.json"); ;
        var dataStore = new JsonFileDataStore<Customer>(filepath);
        var customerrepo = new CustomerRepository(dataStore);

        while (true)
        {
            Console.WriteLine(@"
--- CUSTOMER ---
1. Create
2. View All
3. Update
4. Delete
0. Back");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Nhập sai!");
                continue;
            }

            switch (choice)
            {
                case 1:
                    customerrepo.Create(Input());
                    break;

                case 2:
                    OutputList(customerrepo.GetAll());
                    break;

                case 3:
                    Console.Write("Nhập ID cần update: ");
                    int updateId = int.Parse(Console.ReadLine());

                    var updated = Input();

                    customerrepo.Update(updated);
                    break;

                case 4:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    customerrepo.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}
