using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Db_Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


public class OrderRepositoryDb : IBaseRepository<Order>
{
    private readonly string _connectionString;
    private readonly SqlDbConnection conn;

    public OrderRepositoryDb(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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
                INSERT INTO [Order] (CustomerId, TotalAmount)
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
                    INSERT INTO OrderDetail (OrderId, ProductId, Quantity, Price)
                    VALUES (@OrderId, @ProductId, @Quantity, @Price)";

                using var cmdDetail = new SqlCommand(insertDetail, condb, tran);

                cmdDetail.Parameters.AddWithValue("@OrderId", orderId);
                cmdDetail.Parameters.AddWithValue("@ProductId", detail.ProductId);
                cmdDetail.Parameters.AddWithValue("@Quantity", detail.Quantity);
                cmdDetail.Parameters.AddWithValue("@Price", detail.Price);

                cmdDetail.ExecuteNonQuery();

                // Update stock
                string updateStock = @"
                    UPDATE Product
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
            string selectDetails = "SELECT * FROM OrderDetail WHERE OrderId = @OrderId";
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
            string deleteDetails = "DELETE FROM OrderDetail WHERE OrderId = @OrderId";
            using (var cmdDeleteDetails = new SqlCommand(deleteDetails, condb, tran))
            {
                cmdDeleteDetails.Parameters.AddWithValue("@OrderId", id);
                cmdDeleteDetails.ExecuteNonQuery();
            }
            // 3. Delete Order
            string deleteOrder = "DELETE FROM [Order] WHERE OrderId = @OrderId";
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
                    UPDATE Product
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
            return false;
        }
    }

    public IReadOnlyList<Order> GetAll()
    {
       using var condb = conn.GetConnection();
        condb.Open();
        string query = @"  SELECT *  FROM [Order] o
            LEFT JOIN OrderDetail od ON o.OrderId = od.OrderId";
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
            SELECT * FROM [Order] o LEFT JOIN OrderDetail od ON o.OrderId = od.OrderId
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

public class OrderRepositoryDbContext:IBaseRepository<Order>

{
    private readonly ShopDbContext _dbContext;
    private readonly string _conn;
        public OrderRepositoryDbContext(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _dbContext = new ShopDbContext(_conn);
        }
    public void Create(Order order)
    {
        using var tran = _dbContext.Database.BeginTransaction();
        try
        {
            foreach (var detail in order.Details)
            {
                var product = _dbContext.Products.Find(detail.ProductId);
                if(product == null)
                    throw new Exception($"Product with ID {detail.ProductId} not found");

                if (product.StockQuantity < detail.Quantity)
                    throw new Exception("Not enough stock");

                product.StockQuantity -= detail.Quantity;
            }

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            tran.Commit();
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }
    public bool Delete(int id)
    {   using var tran = _dbContext.Database.BeginTransaction();
        try {

            var order = GetById(id);
            if (order == null) return false;
            foreach (var detail in order.Details)
            {
                var product = _dbContext.Products.Find(detail.ProductId);
                if(product == null)
                    throw new Exception($"Product with ID {detail.ProductId} not found");
                product.StockQuantity += detail.Quantity;
            }
            _dbContext.Orders.Remove(order);
            _dbContext.SaveChanges();
            tran.Commit();
            return true;
        }
        catch
        {   
            tran.Rollback();
            return false;
        }
    }
    public IReadOnlyList<Order> GetAll()
    => _dbContext.Orders.ToList();
    public Order? GetById(int id)
    => _dbContext.Orders.Find(id);


    public bool Update(Order entity)
    {
        throw new NotSupportedException("Order does not support update");
    }


}