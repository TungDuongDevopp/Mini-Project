using Application.Interface;
using Domain.Entity;

namespace Infrastructure.Repository;

public class StaffRepositoryFile: IBaseRepository<Staff>
{
    private List<Staff> _staff = new();
    private readonly IDataStore<Staff> _dataStore;
    public StaffRepositoryFile (IDataStore<Staff> dataStore)
    {
        _dataStore = dataStore;
        _staff = _dataStore.Load();
    }
    public void Create(Staff entity)

    {   entity.StaffId = _staff.Any() ? _staff.Max(x => x.StaffId) + 1 : 1;
        _staff.Add(entity);
        _dataStore.Save(_staff);
    }

    public bool Delete(int id)
    {
        var staff = GetById(id);
        if (staff != null)
        {
            _staff.Remove(staff);
            _dataStore.Save(_staff);
            return true;
        }
        return false;
    }

    public IReadOnlyList<Staff> GetAll()
    => _staff.ToList();
    

    public Staff? GetById(int id)
    => _staff.FirstOrDefault(x => x.StaffId == id);
        
    

    public bool Update(Staff entity)
    {
        var existing = GetById(entity.StaffId);
        if (existing != null)
        {
            existing.Name = entity.Name;
            existing.Position = entity.Position;
            existing.Salary = entity.Salary;
            _dataStore.Save(_staff);
            return true;

        }
        return false;
    }
}
