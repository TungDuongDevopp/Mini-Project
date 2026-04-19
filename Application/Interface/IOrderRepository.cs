

using Domain.Entity;

namespace Application.Interface;

public interface IOrderRepository
{
    void CreateOrderWithDetails(Order order);
}
