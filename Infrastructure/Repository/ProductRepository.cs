using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;


namespace Infrastructure.Repository;

public class ProductRepositoryFile : IProductRepository
{
    private List<Product> _products = new();
    private readonly IDataStore<Product> _dataStore;
    public ProductRepositoryFile(IDataStore<Product> dataStore)
    {
        _dataStore = dataStore;
        _products = _dataStore.Load();
    }
    public void Create(Product product)

    {
        product.ProductId = _products.Any() ? _products.Max(x => x.ProductId) + 1 : 1;

        _products.Add(product);
        _dataStore.Save(_products);

    }

    public bool Update(Product product)
    {
        var existing = GetById(product.ProductId);
        if (existing == null) return false;
        else
        {
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;
        }
        _dataStore.Save(_products);
        return true;
    }


    public bool Delete(int id)
    {
        var product = GetById(id);
        if (product == null) return false;
        _products.Remove(product);
        _dataStore.Save(_products);
        return true;

    }
    public IReadOnlyList<Product> GetAll()
    => _products.ToList();
    
    public Product? GetById(int id)
      => _products.FirstOrDefault(x => x.ProductId == id);
    
    public void DecreaseStock(int productId, int quantity)
    {
        var product = GetById(productId);

        if (product == null)
            throw new Exception("Product not found");
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");
        if (product.StockQuantity < quantity)
            throw new InvalidOperationException("Not enough stock");

        product.StockQuantity -= quantity;

        _dataStore.Save(_products);
    }
}


public class ProductRepositoryDb : IProductRepository

    
{
    private readonly SqlDbConnection conn;
    private readonly string _connectionString;
    public ProductRepositoryDb(string connectionString)
    {
        _connectionString = connectionString;
        conn = new SqlDbConnection(_connectionString);
    }
    public void Create(Product entity)
    {
        string query = "INSERT INTO Products (Name, Description, Price, StockQuantity) VALUES (@Name, @Description, @Price, @StockQuantity)";
        using var condb = conn.GetConnection();
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Description", entity.Description);
        cmd.Parameters.AddWithValue("@Price", entity.Price);
        cmd.Parameters.AddWithValue("@StockQuantity", entity.StockQuantity);
        condb.Open();
        cmd.ExecuteNonQuery();
    }

    public void DecreaseStock(int productId, int quantity)
    {
        using var condb = conn.GetConnection();
        var product = GetById(productId);
        if (product == null)
            throw new Exception("Product not found");
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");
        if (product.StockQuantity < quantity)
            throw new InvalidOperationException("Not enough stock");
        string query = "UPDATE Products SET StockQuantity = StockQuantity - @Quantity WHERE ProductId = @ProductId";
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Quantity", quantity);
        cmd.Parameters.AddWithValue("@ProductId", productId);
        condb.Open();   
        cmd.ExecuteNonQuery();
    }

    public bool Delete(int id)
    {
        string query = "DELETE FROM Products WHERE ProductId = @ProductId";
        using var condb = conn.GetConnection();
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@ProductId", id);
        condb.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    public IReadOnlyList<Product> GetAll()
    {
        var products = new List<Product>();
        using var condb = conn.GetConnection();
        string query = "SELECT * FROM Products";
        using var cmd = new SqlCommand(query, condb);
        condb.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            products.Add(new Product
            {
                ProductId = (int)reader["ProductId"],
                Name = reader["Name"].ToString() ?? string.Empty,
                Description = reader["Description"].ToString() ?? string.Empty,
                Price = (decimal)reader["Price"],
                StockQuantity = (int)reader["StockQuantity"]
            });
        }
        return products;
    }

    public Product? GetById(int id)
    {
        string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
        using var condb = conn.GetConnection();
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@ProductId", id);
        condb.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Product
            {
                ProductId = (int)reader["ProductId"],
                Name = reader["Name"].ToString() ?? string.Empty,
                Description = reader["Description"].ToString() ?? string.Empty,
                Price = (decimal)reader["Price"],
                StockQuantity = (int)reader["StockQuantity"]
            };
        }
        return null;
    }

    public bool Update(Product entity)
    {
        using var condb = conn.GetConnection();
        string query = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, StockQuantity = @StockQuantity WHERE ProductId = @ProductId";
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Description", entity.Description);
        cmd.Parameters.AddWithValue("@Price", entity.Price);
        cmd.Parameters.AddWithValue("@StockQuantity", entity.StockQuantity);
        cmd.Parameters.AddWithValue("@ProductId", entity.ProductId);
        condb.Open();   
        return cmd.ExecuteNonQuery() > 0;
        

    }
}