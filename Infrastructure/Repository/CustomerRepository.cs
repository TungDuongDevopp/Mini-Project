using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;


namespace Infrastructure.Repository;
public class CustomerRepositoryFile : IBaseRepository<Customer>
{
    private List<Customer> _customers = new();
    private readonly IDataStore<Customer> _dataStore;

    public CustomerRepositoryFile(IDataStore<Customer> dataStore)
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


public class CustomerRepositoryDb : IBaseRepository<Customer>
{
    private readonly string _connectionString;
    private readonly SqlDbConnection conn;
    public CustomerRepositoryDb(string connectionString)
    {
        _connectionString = connectionString;
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