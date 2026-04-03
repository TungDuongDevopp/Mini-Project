using Domain.Entity;

namespace UI.Handler;

internal class OrderConsoleHander : IConsoleHandler<Order>

{
    public Order Input()
    {
        
    }

    public void Output(Order entity)
    {
        throw new NotImplementedException();
    }

    public void OutputList(IEnumerable<Order> list)
    {
        throw new NotImplementedException();
    }
}
