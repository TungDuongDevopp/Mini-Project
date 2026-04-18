
using Application.Interface;
using Domain.Entity;

namespace Infrastructure.Repository.Database;

public class StaffRepositoryDb : IBaseRepository<Staff>
{
    public void Create(Staff entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Staff> GetAll()
    {
        throw new NotImplementedException();
    }

    public Staff? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Update(Staff entity)
    {
        throw new NotImplementedException();
    }
}
