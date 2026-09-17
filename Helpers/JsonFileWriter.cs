namespace portfolio_api.Helpers;

using System.Text.Encodings.Web;
using System.Text.Json;

public class JsonFileWriter<T>
{
    private readonly string _filePath;

    public JsonFileWriter(string fileName)
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    }

    public async Task WriteAsync(T data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true,
            IndentSize = 4,
            IndentCharacter = ' ',
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        await File.WriteAllTextAsync(_filePath, json);
    }
}