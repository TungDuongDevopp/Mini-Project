using Application.Interface;
using Domain.Entity;

namespace Application.Service;

public class CustomerService : IBaseRepository<Customer>
{
    private List<Customer> _customers = new();
    public void Create(Customer entity)
    {
        _customers.Add(entity);
    }

    public bool Delete(int id)
    {
        var customer = _customers.FirstOrDefault(x => x.CustomerId == id);
       if (customer==null) return false;
       _customers.Remove(customer);
        return true;
    }

    public IReadOnlyList<Customer> GetAll()
    => _customers;



    public Customer? GetById(int id)
    {
        var customer = _customers.FirstOrDefault(x => x.CustomerId == id);
        return customer;
    }
    public bool Update(Customer entity)
    {
        var exsisting = _customers.FirstOrDefault(x=> x.CustomerId==entity.CustomerId);
        if (exsisting == null) return false;
        else
        {
            exsisting.Name = entity.Name;
            exsisting.PhoneNumber = entity.PhoneNumber;
            exsisting.Email = entity.Email;
        }
        return true;
    }
}
