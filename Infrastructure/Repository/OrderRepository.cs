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
public class OrderRepositoryDb : IBaseRepository<Order>
{
    private readonly string _connectionString;
    private readonly SqlDbConnection conn;

    public OrderRepositoryDb(string connectionString)
    {
        _connectionString = connectionString;
        conn = new SqlDbConnection(_connectionString);
    }

    public void Create(Order order)
    {
        
        using var condb = conn.GetConnection();
        condb.Open();

        using var tran = condb.BeginTransaction();

        try
        {
            // 1. Insert Order
            string insertOrder = @"
                INSERT INTO Orders (CustomerId, TotalAmount)
                VALUES (@CustomerId, @TotalAmount);
                SELECT SCOPE_IDENTITY();";

            using var cmdOrder = new SqlCommand(insertOrder, condb, tran);

            cmdOrder.Parameters.AddWithValue("@CustomerId", order.CustomerId);
            cmdOrder.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

            int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

            // 2. Insert OrderDetails + Update Stock
            foreach (var detail in order.Details)
            {
                // Insert detail
                string insertDetail = @"
                    INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price)
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

  

    public bool Delete(int id)
    {
       using var condb = conn.GetConnection();
        condb.Open();
        using var tran = condb.BeginTransaction();
        try
        {
            // 1. Get OrderDetails to restore stock
            string selectDetails = "SELECT ProductId, Quantity FROM OrderDetails WHERE OrderId = @OrderId";
            List<(int ProductId, int Quantity)> details = new();
            using (var cmdSelect = new SqlCommand(selectDetails, condb, tran))
            {
                cmdSelect.Parameters.AddWithValue("@OrderId", id);
                using var reader = cmdSelect.ExecuteReader();
                while (reader.Read())
                {
                    details.Add((reader.GetInt32(0), reader.GetInt32(1)));
                }
            }
            // 2. Delete OrderDetails
            string deleteDetails = "DELETE FROM OrderDetails WHERE OrderId = @OrderId";
            using (var cmdDeleteDetails = new SqlCommand(deleteDetails, condb, tran))
            {
                cmdDeleteDetails.Parameters.AddWithValue("@OrderId", id);
                cmdDeleteDetails.ExecuteNonQuery();
            }
            // 3. Delete Order
            string deleteOrder = "DELETE FROM Orders WHERE OrderId = @OrderId";
            using (var cmdDeleteOrder = new SqlCommand(deleteOrder, condb, tran))
            {
                cmdDeleteOrder.Parameters.AddWithValue("@OrderId", id);
                int rowsAffected = cmdDeleteOrder.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    tran.Rollback();
                    return false; // No order deleted
                }
            }
            // 4. Restore stock
            foreach (var detail in details)
            {
                string updateStock = @"
                    UPDATE Products
                    SET StockQuantity = StockQuantity + @Quantity
                    WHERE ProductId = @ProductId";
                using var cmdStock = new SqlCommand(updateStock, condb, tran);
                cmdStock.Parameters.AddWithValue("@Quantity", detail.Quantity);
                cmdStock.Parameters.AddWithValue("@ProductId", detail.ProductId);
                cmdStock.ExecuteNonQuery();
            }
            tran.Commit();
            return true;
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }

    public IReadOnlyList<Order> GetAll()
    {
       using var condb = conn.GetConnection();
        condb.Open();
        string query = @"
            SELECT o.OrderId, o.CustomerId, o.OrderDate, o.TotalAmount,
                   od.OrderDetailId, od.ProductId, od.Quantity, od.UnitPrice
            FROM Orders o
            LEFT JOIN OrderDetails od ON o.OrderId = od.OrderId";
        var ordersDict = new Dictionary<int, Order>();
        using var cmd = new SqlCommand(query, condb);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int orderId = (int)reader["OrderId"];
            if (!ordersDict.TryGetValue(orderId, out var order))
            {
                order = new Order
                {
                    OrderId = (int)reader["OrderId"],
                    CustomerId = (int)reader["CustomerId"],
                    TotalAmount = (decimal)reader["TotalAmount"],
                    Details = new List<OrderDetail>()
                };
                ordersDict[orderId] = order;
            }
            if (!reader.IsDBNull(4)) // Check if OrderDetail exists
            {
                order.Details.Add(new OrderDetail
                {
                    OrderDetailId = (int)reader["OrderDetailId"],
                    ProductId = (int)reader["ProductId"],
                    Quantity = (int)reader["Quantity"],
                    Price = (decimal)reader["Price"]
                });
            }
        }
        return ordersDict.Values.ToList();
    }

    public Order? GetById(int id)
    {
       using var condb = conn.GetConnection();
        condb.Open();
        string query = @"
            SELECT o.OrderId, o.CustomerId, o.OrderDate, o.TotalAmount,
                   od.OrderDetailId, od.ProductId, od.Quantity, od.UnitPrice
            FROM Orders o
            LEFT JOIN OrderDetails od ON o.OrderId = od.OrderId
            WHERE o.OrderId = @OrderId";
        Order? order = null;
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@OrderId", id);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            if (order == null)
            {
                order = new Order
                {
                    OrderId = (int)reader["OrderId"],
                    CustomerId = (int)reader["CustomerId"],
                    TotalAmount = (decimal)reader["TotalAmount"],
                    Details = new List<OrderDetail>()
                };
            }
            if (!reader.IsDBNull(4)) // Check if OrderDetail exists
            {
                order.Details.Add(new OrderDetail
                {
                    OrderDetailId = (int)reader["OrderDetailId"],
                    ProductId = (int)reader["ProductId"],
                    Quantity = (int)reader["Quantity"],
                    Price = (decimal)reader["Price"]
                });
            }
        }
        return order;
    }

    public bool Update(Order entity)
    {
        throw new NotSupportedException("Order does not support update");
    }
}

