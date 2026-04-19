using Domain.Entity;

namespace Application.Interface
{
   public interface IOrder
    {
        IReadOnlyList<Order> GetAll();

        Order ? GetById(int id);
    
        void Create(int customerId , List<(int productId,int quantity)> items);
    
    
       bool Delete(int id);
    }
}
