

using Application.Interface;
using Domain.Entity;

namespace Application.Service;

public class StaffService : IBaseRepository<Staff>
{
    private List<Staff> _staff = new();
    public void Create(Staff entity)
    {
        _staff.Add(entity);
    }

    public bool Delete(int id)
    {
        var staff = GetById(id);    
        if (staff != null)
        {
            _staff.Remove(staff);
            return true;
        }
        return false;
    }

    public IReadOnlyList<Staff> GetAll()
    {
        return _staff;
    }

    public Staff? GetById(int id)
    {
        var staff = _staff.FirstOrDefault(x => x.StaffId == id);
        return staff;
    }

    public bool Update(Staff entity)
    {
        var existing = GetById(entity.StaffId);
        if (existing != null)
        {
            existing.Name = entity.Name;
            existing.Position = entity.Position;
            existing.Salary = entity.Salary;
            return true;
        }
        return false;
    }
}
