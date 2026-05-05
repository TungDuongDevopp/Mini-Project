using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Db_Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.Repository;

public class ProductRepositoryDb : IProductRepository

    
{
    private readonly SqlDbConnection conn;
    private readonly string _connectionString;
    public ProductRepositoryDb(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        conn = new SqlDbConnection(_connectionString);
    }
    public void Create(Product entity)
    {
        string query = "INSERT INTO Product (Name, Description, Price, StockQuantity) VALUES (@Name, @Description, @Price, @StockQuantity)";
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
        string query = "DELETE FROM Product WHERE ProductId = @ProductId";
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
        string query = "SELECT * FROM Product";
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
        string query = "SELECT * FROM Product WHERE ProductId = @ProductId";
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
        string query = "UPDATE Product SET Name = @Name, Description = @Description, Price = @Price, StockQuantity = @StockQuantity WHERE ProductId = @ProductId";
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

public class ProductRepositoryDbConext : IProductRepository

{
    private readonly ShopDbContext _dbContext;
    private readonly string _conn;
    public ProductRepositoryDbConext(IConfiguration config)
    {
        _conn = config.GetConnectionString("DefaultConnection")?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _dbContext = new ShopDbContext(_conn);
    }

    public void Create(Product entity)
    {
       _dbContext.Products.Add(entity);
        _dbContext.SaveChanges();
    }

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
        _dbContext.SaveChanges();
    }

    public bool Delete(int id)
    {
        var product = GetById(id);
        if (product == null) return false;
        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();
        return true;
    }

    public IReadOnlyList<Product> GetAll()
   => _dbContext.Products.ToList();

    public Product? GetById(int id)
    => _dbContext.Products.Find(id);

    public bool Update(Product entity)
    {
        var existing = GetById(entity.ProductId);
        if (existing == null) return false;
        existing.Name = entity.Name;
        existing.Description = entity.Description;
        existing.Price = entity.Price;
        existing.StockQuantity = entity.StockQuantity;
        _dbContext.SaveChanges();
        return true;
    }
}