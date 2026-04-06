using Application.Interface;
using System.Text.Json;

namespace Infrastructure.Data;

public class JsonFileDataStore<T> : IDataStore<T>

{
    private readonly string _filePath;

    public JsonFileDataStore(string filePath)
    {
        _filePath = filePath;
    }
    public List<T> Load()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<T>();
        }

        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public void Save(List<T> data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_filePath, json);
    }
}
