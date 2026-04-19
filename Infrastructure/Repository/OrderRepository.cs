using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;


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
public class OrderRepositoryDb : IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepositoryDb(string connectionString)
    {
        _connectionString = connectionString;
    }

  

    public void CreateOrderWithDetails(Order order)
    {
        var conn = new SqlDbConnection(_connectionString);
        using var condb = conn.GetConnection();
        condb.Open();

        using var tran = condb.BeginTransaction();

        try
        {
            // 1. Insert Order
            string insertOrder = @"
                INSERT INTO Orders (CustomerId, OrderDate, TotalAmount)
                VALUES (@CustomerId, @OrderDate, @TotalAmount);
                SELECT SCOPE_IDENTITY();";

            using var cmdOrder = new SqlCommand(insertOrder, condb, tran);

            cmdOrder.Parameters.AddWithValue("@CustomerId", order.CustomerId);
            cmdOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now);
            cmdOrder.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

            int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

            // 2. Insert OrderDetails + Update Stock
            foreach (var detail in order.Details)
            {
                // Insert detail
                string insertDetail = @"
                    INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice)
                    VALUES (@OrderId, @ProductId, @Quantity, @Price)";

                using var cmdDetail = new SqlCommand(insertDetail, condb, tran);

                cmdDetail.Parameters.AddWithValue("@OrderId", orderId);
                cmdDetail.Parameters.AddWithValue("@ProductId", detail.ProductId);
                cmdDetail.Parameters.AddWithValue("@Quantity", detail.Quantity);
                cmdDetail.Parameters.AddWithValue("@Price", detail.Price);

                cmdDetail.ExecuteNonQuery();

                // Update stock
                string updateStock = @"
                    UPDATE Products
                    SET StockQuantity = StockQuantity - @Quantity
                    WHERE ProductId = @ProductId";

                using var cmdStock = new SqlCommand(updateStock, condb, tran);

                cmdStock.Parameters.AddWithValue("@Quantity", detail.Quantity);
                cmdStock.Parameters.AddWithValue("@ProductId", detail.ProductId);

                cmdStock.ExecuteNonQuery();
            }

            tran.Commit();
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }

   

   
}

