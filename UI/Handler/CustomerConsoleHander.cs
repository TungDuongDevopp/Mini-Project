using Application.Service;
using Domain.Entity;
using UI.Helper;
using static InputHelper;

namespace UI.Handler;
internal class CustomerConsoleHander: IConsoleHandler<Customer>
{
    private readonly CustomerService _customerService;

    public CustomerConsoleHander (CustomerService customerService)
    {
        _customerService = customerService;
    }

    public Customer Input()
    {
        string name = InputHelper.Input("Enter Customer Name:", Parsers.String);
        string phoneNumber = InputHelper.Input("Enter Customer Phone Number:", Parsers.String,v=> Validator.IsValidPhone(v)&& _customerService.IsPhoneUnique(v));
        string email = InputHelper.Input("Enter Customer Email:", Parsers.String, v => Validator.IsValidEmail(v) && _customerService.IsEmailUnique(v));
        return new Customer
        {
         
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
                    _customerService.Create(Input());
                    break;

                case 2:
                    OutputList(_customerService.GetAll());
                    break;

                case 3:
                    Console.Write("Nhập ID cần update: ");
                    int updateId = int.Parse(Console.ReadLine() ?? "0");

                    var existing = _customerService.GetById(updateId);

                    if (existing != null)
                    {
                        existing.Name = InputHelper.Input("Enter Customer Name:", Parsers.String);
                        existing.PhoneNumber = InputHelper.Input("Enter Customer Phone Number:", Parsers.String,
                            v => Validator.IsValidPhone(v) && _customerService.IsPhoneUnique(v, updateId));

                        existing.Email = InputHelper.Input("Enter Customer Email:", Parsers.String,
                            v => Validator.IsValidEmail(v) && _customerService.IsEmailUnique(v, updateId));

                        _customerService.Update(existing);
                    }
                    else
                    {
                        Console.WriteLine("Không tìm thấy khách hàng với ID này.");
                    }
                    break;
                case 4:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine() ?? "0");
                    if (_customerService.GetById(deleteId) == null)
                    {
                        Console.WriteLine("Không tìm thấy khách hàng với ID này.");
                        break;
                    }
                    _customerService.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}
