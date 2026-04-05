using Application.Interface;
using System.Text.Json;

public class FileRepository<T> : IBaseRepository<T> where T : class
{
    private readonly string _filePath;

    public FileRepository(string filePath)
    {
        _filePath = filePath;
    }

    private List<T> ReadFile()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        var json = File.ReadAllText(_filePath);

        return string.IsNullOrWhiteSpace(json)
            ? new List<T>()
            : JsonSerializer.Deserialize<List<T>>(json)!;
    }

    private void WriteFile(List<T> data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }

    // 🔹 Lấy Id bằng reflection (tạm chấp nhận)
    private int GetId(T entity)
    {
        var prop = typeof(T).GetProperties()
            .FirstOrDefault(p => p.Name.EndsWith("Id"));

        if (prop == null)
            throw new Exception("No Id property found");

        return (int)prop.GetValue(entity)!;
    }

    public void Create(T entity)
    {
        var data = ReadFile();
        data.Add(entity);
        WriteFile(data);  
    }

    public IReadOnlyList<T> GetAll()
    {
        return ReadFile();
    }

    public T? GetById(int id)
    {
        var data = ReadFile();

        return data.FirstOrDefault(x => GetId(x) == id);
    }

    public bool Update(T entity)
    {
        var data = ReadFile();

        var id = GetId(entity);

        var index = data.FindIndex(x => GetId(x) == id);

        if (index == -1)
            return false;

        data[index] = entity;

        WriteFile(data);
        return true;
    }

    public bool Delete(int id)
    {
        var data = ReadFile();

        var entity = data.FirstOrDefault(x => GetId(x) == id);

        if (entity == null)
            return false;

        data.Remove(entity);
        WriteFile(data);

        return true;
    }
}