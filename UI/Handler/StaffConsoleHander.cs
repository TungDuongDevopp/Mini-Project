using Domain.Entity;

namespace UI.Handler;

internal class StaffConsoleHander : IConsoleHandler<Staff>
{
    public Staff Input()
    {
        Console.WriteLine("Enter Staff Id:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter Staff Name:");
        string name = Console.ReadLine()!;
        Console.WriteLine("Enter Position:");
        string position = Console.ReadLine()!;
        Console.WriteLine("Enter Salary");
       decimal salary = decimal.Parse(Console.ReadLine()!);
        return new Staff
        {
            StaffId = id,
            Name = name,
            Position = position,
            Salary = salary
        };
    }

    public void Output(Staff entity)
    {
        Console.WriteLine($"{entity.StaffId} - {entity.Name} - {entity.Position} - {entity.Salary}");

    }

    public void OutputList(IEnumerable<Staff> list)
    {
        foreach (var staff in list)
        {
            Output(staff);
        }
    }
}
