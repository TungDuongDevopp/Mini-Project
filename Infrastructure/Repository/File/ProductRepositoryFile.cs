using Application.Interface;
using Domain.Entity;


namespace Infrastructure.Repository.File;

public class ProductRepositoryFile: IProductRepository
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
