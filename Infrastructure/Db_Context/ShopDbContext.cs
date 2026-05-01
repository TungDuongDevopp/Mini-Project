using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace Infrastructure.Db_Context;

public class ShopDbContext : DbContext
{
    private readonly string conn;
    public ShopDbContext(string connectionString)
    {
        conn = connectionString;
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    { base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(conn);
    }


    public void CreateDatabase(ShopDbContext context)
    {
        context.Database.EnsureCreated();

    }

    public void DeleteDatabase(ShopDbContext context)
    {
        context.Database.EnsureDeleted();
    }
}