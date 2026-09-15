namespace portfolio_api.Helpers;

using System.Text.Json;

public class JsonFileReader<T>
{
    private readonly string _filePath;

    public JsonFileReader(string fileName)
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    }

    public async Task<T> ReadAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Failed to deserialize JSON");
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"File not found: {_filePath}");
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Error deserializing JSON from {_filePath}: {ex.Message}", ex);
        }
    }
}
