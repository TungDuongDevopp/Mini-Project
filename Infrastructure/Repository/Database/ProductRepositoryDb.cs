
using Application.Interface;
using Domain.Entity;

namespace Infrastructure.Repository.Database;

public class ProductRepositoryDb : IProductRepository
{
    public void Create(Product entity)
    {
        throw new NotImplementedException();
    }

    public void DecreaseStock(int productId, int quantity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Product> GetAll()
    {
        throw new NotImplementedException();
    }

    public Product? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public bool Update(Product entity)
    {
        throw new NotImplementedException();
    }
}
