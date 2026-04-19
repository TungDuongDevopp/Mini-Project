using Application.Interface;
using Domain.Entity;


public class OrderRepositoryFile : IBaseRepository<Order>
{
    private List<Order> _orders = new();
    private readonly IDataStore<Order> _dataStore;

    public OrderRepositoryFile(IDataStore<Order> dataStore)
    {
        _dataStore = dataStore;
        _orders = _dataStore.Load();
    }

    public void Create(Order order)
    {
        order.OrderId = _orders.Any() ? _orders.Max(x => x.OrderId) + 1 : 1;

        _orders.Add(order);
        _dataStore.Save(_orders);
    }

    public Order? GetById(int id)
        => _orders.FirstOrDefault(o => o.OrderId == id);

    public IReadOnlyList<Order> GetAll()
        => _orders.ToList();

    public bool Delete(int id)
    {
        var order = GetById(id);
        if (order == null) return false;

        _orders.Remove(order);
        _dataStore.Save(_orders);
        return true;
    }

    public bool Update(Order entity)
    {
        throw new NotSupportedException("Order does not support update");
    }
}


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
