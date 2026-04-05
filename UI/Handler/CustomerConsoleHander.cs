
using Domain.Entity;

namespace UI.Handler;

internal class CustomerConsoleHander: IConsoleHandler<Customer>
{
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
}
