using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Db_Context;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.Repository;

public class CustomerRepositoryDb : IBaseRepository<Customer>
{
    private readonly string _connectionString;
    private readonly SqlDbConnection conn;
    public CustomerRepositoryDb(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        conn = new SqlDbConnection(_connectionString);
    }

    public void Create(Customer entity)

    {
      
        using var condb = conn.GetConnection();
        
        string querry = @"INSERT INTO Customer (Name, Email, PhoneNumber) 
                         VALUES (@Name, @Email, @PhoneNumber)";
        using var cmd = new SqlCommand(querry, condb);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Email", entity.Email);
        cmd.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
        condb.Open();
        cmd.ExecuteNonQuery();
    }

    public bool Delete(int id)
    {
        using var condb = conn.GetConnection();
        string query = "DELETE FROM Customer WHERE CustomerId = @Id";

        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Id", id);
        condb.Open();
        return cmd.ExecuteNonQuery() > 0;

        
    }

    public IReadOnlyList<Customer> GetAll()
    {
        var customers = new List<Customer>();
        using var condb = conn.GetConnection();
        string query = "SELECT * FROM Customer";

        using var cmd = new SqlCommand(query, condb);

        condb.Open();
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            customers.Add(new Customer
            {
                CustomerId = (int)reader["CustomerId"],
                Name = reader["Name"].ToString() ?? string.Empty,
                PhoneNumber = reader["PhoneNumber"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty
            });
        }

        return customers;
    }

    public Customer? GetById(int id)
    {
        using var condb = conn.GetConnection();
        string query = @"SELECT * FROM Customer WHERE CustomerId = @Id";

        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Id", id);
        condb.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Customer
            {
                CustomerId = (int)reader["CustomerId"],
                Name = reader["Name"].ToString() ?? string.Empty,
                PhoneNumber = reader["PhoneNumber"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty
            };
        }

        return null;
    }

    public bool Update(Customer entity)
    {
        string query = @"UPDATE Customer 
                         SET Name = @Name, Email = @Email, PhoneNumber = @PhoneNumber
                         WHERE CustomerId = @Id";
        using var condb = conn.GetConnection();
        using var cmd = new SqlCommand(query, condb);
      
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Email", entity.Email);
        cmd.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
        cmd.Parameters.AddWithValue("@Id", entity.CustomerId);
        condb.Open();
        return cmd.ExecuteNonQuery() > 0;
     


    }
}

public class CustomerRepositoryDbContext : IBaseRepository<Customer>
{
        private readonly string _conn;
        private readonly ShopDbContext _dbContext;
        public CustomerRepositoryDbContext(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _dbContext = new ShopDbContext(_conn);
        }


    
    public void Create(Customer entity)
    {
        _dbContext.Customers.Add(entity);
        _dbContext.SaveChanges();
    }

    public bool Delete(int id)
    {
        var existing = GetById(id);
        if (existing == null) return false;
         _dbContext.Customers.Remove(existing);
        _dbContext.SaveChanges();
        return true;
    }

    public IReadOnlyList<Customer> GetAll()
    => _dbContext.Customers.ToList();
    

    public Customer? GetById(int id)
      => _dbContext.Customers.Find(id);
      
    

    public bool Update(Customer entity)
    {
        var existing = GetById(entity.CustomerId);
        if (existing == null) return false;
        existing.Name = entity.Name;
        existing.Email = entity.Email;
        existing.PhoneNumber = entity.PhoneNumber;
        _dbContext.SaveChanges();
        return true;
    }
}