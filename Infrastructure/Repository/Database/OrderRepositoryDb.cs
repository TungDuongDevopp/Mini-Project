
using Application.Interface;
using Domain.Entity;

namespace Infrastructure.Repository.Database;

public class OrderRepositoryDb : IBaseRepository<Order>
{
    public void Create(Order entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Order> GetAll()
    {
        throw new NotImplementedException();
    }

    public Order? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Update(Order entity)
    {
        throw new NotImplementedException();
    }
}
