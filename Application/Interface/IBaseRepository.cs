namespace Application.Interface;

public interface IBaseRepository<T> where T : class
{
    void Create(T entity);

    bool Update(T entity);

    bool Delete(int id);

    IReadOnlyList<T> GetAll();

    T? GetById(int id);
}