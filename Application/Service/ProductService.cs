using Application.Interface;
using Domain.Entity;

namespace Application.Service;

public class ProductService: IBaseRepository<Product>
{
    private List<Product> _products = new();
    public void Create(Product product) =>  _products.Add(product);
    
    public bool Update(Product product)
    {
        var existing = _products.FirstOrDefault(x => x.ProductId == product.ProductId);
        if (existing == null) return false;
        else {
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;
        }
           
        return true;
    }
    

    public bool Delete(int id)
    {
        var product = _products.FirstOrDefault(x=>x.ProductId == id);
        if (product == null) return false;
        _products.Remove(product);
        return true;

    }
    public IReadOnlyList<Product> GetAll()
    {
        return _products;
    }
    public Product ? GetById(int id)
    {
        var product = _products.FirstOrDefault(x => x.ProductId == id);
        return product;
    }
    
}
