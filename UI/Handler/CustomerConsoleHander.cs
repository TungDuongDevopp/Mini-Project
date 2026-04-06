
using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace UI.Handler;

internal class CustomerConsoleHander: IConsoleHandler<Customer>
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
    public Customer Input()
    {
        Console.WriteLine("Enter Customer Id:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Customer Name:");
        string name = Console.ReadLine()!;
        Console.WriteLine("Enter Customer Phone Number:");
        string phoneNumber = Console.ReadLine()!;
        Console.WriteLine("Enter Customer Email:");
        string email = Console.ReadLine()!;
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
        Console.WriteLine($"{entity.CustomerId} - {entity.Name} - {entity.Email} - {entity.PhoneNumber}");
    }

    public void OutputList(IEnumerable<Customer> list)
    {
       foreach (var customer in list)
       {
           Output(customer);
        }
    }

    public void Run()
       
    {
        var filepath = Path.Combine(BasePath, "Data", "Customer.json"); ;
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
                    updated.CustomerId = updateId;

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
