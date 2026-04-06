using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace UI.Handler;

internal class StaffConsoleHander : IConsoleHandler<Staff>
{
    private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;
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

    public void Run()
    {
        var filepath = Path.Combine(BasePath, "Data", "Staff.json");
        var dataStore = new JsonFileDataStore<Staff>(filepath);
        var staffrepo = new StaffRepository(dataStore);

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
                    staffrepo.Create(Input());
                    break;

                case 2:
                    OutputList(staffrepo.GetAll());
                    break;

                case 3:
                    Console.Write("Nhập ID cần update: ");
                    int updateId = int.Parse(Console.ReadLine());

                    var updated = Input();
                    updated.StaffId= updateId;

                    staffrepo.Update(updated);

                    break;

                case 4:
                    Console.Write("Nhập ID cần xóa: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    staffrepo.Delete(deleteId);
                    break;

                case 0:
                    return;
            }
        }
    }
}
