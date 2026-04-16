namespace Application.Interface;

public interface IDbConnection<T> where T: class
{
    public T GetConnection();
}
