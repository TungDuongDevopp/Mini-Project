using Application.Interface;
using Domain.Entity;


namespace Infrastructure.Repository;
public class CustomerRepository : IBaseRepository<Customer>
{
    private List<Customer> _customers = new();
    private readonly IDataStore<Customer> _dataStore;

    public CustomerRepository(IDataStore<Customer> dataStore)
    {
        _dataStore = dataStore;
        _customers = _dataStore.Load();
    }

    public void Create(Customer entity)
    {
        entity.CustomerId = _customers.Any()
            ? _customers.Max(x => x.CustomerId) + 1
            : 1;

        _customers.Add(entity);
        _dataStore.Save(_customers);
    }

    public bool Delete(int id)
    {
        var customer = GetById(id);
        if (customer == null) return false;

        _customers.Remove(customer);
        _dataStore.Save(_customers);
        return true;
    }

    public IReadOnlyList<Customer> GetAll()
        => _customers.ToList();

    public Customer? GetById(int id)
        => _customers.FirstOrDefault(x => x.CustomerId == id);

    public bool Update(Customer entity)
    {
        var existing = GetById(entity.CustomerId);
        if (existing == null) return false;

        existing.Name = entity.Name;
        existing.PhoneNumber = entity.PhoneNumber;
        existing.Email = entity.Email;

        _dataStore.Save(_customers);
        return true;
    }

}