using System.Text.Json;

namespace GymManagement_MVC_Project.DAL.Data.Seeder;

public class SeedJsonLoader
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public static async Task<List<T>> LoadAsync<T>(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

        var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seeder", fileName);
        if (!File.Exists(path))
            throw new FileNotFoundException($"\"{fileName}\" does not exist.");

        await using var fileStream = File.OpenRead(path) ?? throw new Exception("Cannot open the file.");

        return await JsonSerializer.DeserializeAsync<List<T>>(fileStream, _jsonOptions) ?? [];
    }
}
