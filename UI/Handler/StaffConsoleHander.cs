using Application.Service;
using Domain.Entity;
using UI.Helper;

namespace UI.Handler;
internal class StaffConsoleHander : IConsoleHandler<Staff>
{
    private readonly StaffService _staffService;
    public StaffConsoleHander(StaffService staffService)
    {
        _staffService = staffService;
    }

    public Staff Input()
    {
        string name = InputHelper.Input("Enter Staff Name:", InputHelper.Parsers.String);
        string position = InputHelper.Input("Enter Position:", InputHelper.Parsers.String);
        decimal salary = InputHelper.Input("Enter Salary:", InputHelper.Parsers.Decimal,Validator.IsValidMoney);
        return new Staff
        {
            
            Name = name,
            Position = position,
            Salary = salary
        };
    }

    public void Output(Staff entity)
    {
        Console.WriteLine($"{entity.StaffId,-3} | {entity.Name,-25} | {entity.Position,-15} | {entity.Salary.ToString("N0"),-10}");

    }

    public void OutputList(IEnumerable<Staff> list)
       
    {
        Console.WriteLine("Danh sách nhân viên là:");
        Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"{"Id",-3} | {"Name",-25} | {"Position",-15} | {"Salary",-10}");
        foreach (var staff in list)
        {
            Output(staff);
        }
    }

    public void Run()
    {

        while (true)
        {
            Console.WriteLine(@"
--- STAFF ---
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
                    _staffService.Create(Input());
                    break;

                case 2:
                    OutputList(_staffService.GetAll());
                    break;

                case 3:
                    Console.Write("Nhập ID cần update: ");
                    int updateId = int.Parse(Console.ReadLine() ?? "0");

                   var existing = _staffService.GetById(updateId);
                    if (existing == null)
                    {
                        Console.WriteLine("Không tìm thấy nhân viên!");
                        break;
                    }
                    else
                    {
                        existing.Name = InputHelper.Input("Enter Staff Name:", InputHelper.Parsers.String);
                        existing.Position = InputHelper.Input("Enter Position:", InputHelper.Parsers.String);
                        existing.Salary = InputHelper.Input("Enter Salary:", InputHelper.Parsers.Decimal, Validator.IsValidMoney);
                        _staffService.Update(existing);
                    }
                        

                    break;

                case 4:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine() ?? "0");
                    if (_staffService.GetById(deleteId) == null)
                    {
                        Console.WriteLine("Không tìm thấy nhân viên!");
                        break;
                    }
                    _staffService.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}
