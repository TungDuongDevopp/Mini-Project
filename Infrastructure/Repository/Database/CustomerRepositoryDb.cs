
using Application.Interface;
using Domain.Entity;

namespace Infrastructure.Repository.Database;

public class CustomerRepositoryDb : IBaseRepository<Customer>
{
    public void Create(Customer entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Customer> GetAll()
    {
        throw new NotImplementedException();
    }

    public Customer? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Update(Customer entity)
    {
        throw new NotImplementedException();
    }
}
