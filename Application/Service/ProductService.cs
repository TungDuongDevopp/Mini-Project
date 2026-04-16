
using Domain.Entity;

namespace Application.Interface;

public class ProductService : IBaseRepository<Product>
{   private readonly IBaseRepository<Product> _repo;

    public ProductService(IBaseRepository<Product> repo)
    {
        _repo = repo;
    }
    public void Create(Product entity)
      =>  _repo.Create(entity);
    

    public bool Delete(int id)
   => _repo.Delete(id);


    public IReadOnlyList<Product> GetAll()
   => _repo.GetAll().ToList();

    public Product? GetById(int id)
    => _repo.GetById(id);


    public bool Update(Product entity)
   => _repo.Update(entity);
}
