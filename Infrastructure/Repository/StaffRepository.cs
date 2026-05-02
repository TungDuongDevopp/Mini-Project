using Application.Interface;
using Domain.Entity;
using Infrastructure.Data;
using Infrastructure.Db_Context;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repository;

public class StaffRepositoryFile: IBaseRepository<Staff>
{
    private List<Staff> _staff = new();
    private readonly IDataStore<Staff> _dataStore;
    public StaffRepositoryFile (IDataStore<Staff> dataStore)
    {
        _dataStore = dataStore;
        _staff = _dataStore.Load();
    }
    public void Create(Staff entity)

    {   entity.StaffId = _staff.Any() ? _staff.Max(x => x.StaffId) + 1 : 1;
        _staff.Add(entity);
        _dataStore.Save(_staff);
    }

    public bool Delete(int id)
    {
        var staff = GetById(id);
        if (staff != null)
        {
            _staff.Remove(staff);
            _dataStore.Save(_staff);
            return true;
        }
        return false;
    }

    public IReadOnlyList<Staff> GetAll()
    => _staff.ToList();
    

    public Staff? GetById(int id)
    => _staff.FirstOrDefault(x => x.StaffId == id);
        
    

    public bool Update(Staff entity)
    {
        var existing = GetById(entity.StaffId);
        if (existing != null)
        {
            existing.Name = entity.Name;
            existing.Position = entity.Position;
            existing.Salary = entity.Salary;
            _dataStore.Save(_staff);
            return true;

        }
        return false;
    }
}

public class StaffRepositoryDb : IBaseRepository<Staff>
{
    private readonly SqlDbConnection conn;
    private readonly string _connectionString;
    public StaffRepositoryDb(string connectionString)
    {
        _connectionString = connectionString;
        conn = new SqlDbConnection(_connectionString);
    }

    public void Create(Staff entity)
    {
        using var condb = conn.GetConnection();
        string query = "INSERT INTO Staff (Name, Position, Salary) VALUES (@Name, @Position, @Salary)";
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Position", entity.Position);
        cmd.Parameters.AddWithValue("@Salary", entity.Salary);
        condb.Open();
        cmd.ExecuteNonQuery();
    }

    public bool Delete(int id)
    {
        using var condb = conn.GetConnection();
        string query = "DELETE FROM Staff WHERE StaffId = @Id";
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Id", id);
        condb.Open();
        return cmd.ExecuteNonQuery() > 0;
    }

    public IReadOnlyList<Staff> GetAll()
    {
        var staffList = new List<Staff>();
        using var condb = conn.GetConnection();
        string query = "SELECT * FROM Staff";
        using var cmd = new SqlCommand(query, condb);
        condb.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            staffList.Add(new Staff
            {
                StaffId = (int)reader["StaffId"],
                Name = reader["Name"].ToString() ?? string.Empty,
                Position = reader["Position"].ToString() ?? string.Empty,
                Salary = (decimal)reader["Salary"]
            });
        }
        return staffList;
    }

    public Staff? GetById(int id)
    {
       string query = "SELECT * FROM Staff WHERE StaffId = @Id";
        using var condb = conn.GetConnection();
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Id", id);
        condb.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Staff
            {
                StaffId = (int)reader["StaffId"],
                Name = reader["Name"].ToString() ?? string.Empty,
                Position = reader["Position"].ToString() ?? string.Empty,
                Salary = (decimal)reader["Salary"]
            };
        }
        return null;
    }

    public bool Update(Staff entity)
    {
        using var condb = conn.GetConnection();
        string query = "UPDATE Staff SET Name = @Name, Position = @Position, Salary = @Salary WHERE StaffId = @Id";
        using var cmd = new SqlCommand(query, condb);
        cmd.Parameters.AddWithValue("@Name", entity.Name);
        cmd.Parameters.AddWithValue("@Position", entity.Position);
        cmd.Parameters.AddWithValue("@Salary", entity.Salary);
        cmd.Parameters.AddWithValue("@Id", entity.StaffId);
        condb.Open();
        return cmd.ExecuteNonQuery() > 0;
    }
}

public class StaffRepositoryDbContext : IBaseRepository<Staff>
{
    private readonly string _conn;
    private readonly ShopDbContext _dbContext;
    public StaffRepositoryDbContext(string connectionString)
    {
        _conn = connectionString;
        _dbContext = new ShopDbContext(_conn);
    }

    public void Create(Staff entity)
    {
        _dbContext.Staffs.Add(entity);
        _dbContext.SaveChanges();
    }

    public bool Delete(int id)
    {
        var existing = GetById(id);
        if (existing == null) return false;
        _dbContext.Staffs.Remove(existing);
        _dbContext.SaveChanges();
        return true;
    }

    public IReadOnlyList<Staff> GetAll()
        => _dbContext.Staffs.ToList();

    public Staff? GetById(int id)
        => _dbContext.Staffs.Find(id);

    public bool Update(Staff entity)
    {
        var existing = GetById(entity.StaffId);
        if (existing == null) return false;
        existing.Name = entity.Name;
        existing.Position = entity.Position;
        existing.Salary = entity.Salary;
        _dbContext.SaveChanges();
        return true;
    }
}