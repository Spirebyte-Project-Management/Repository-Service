using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spirebyte.Services.Repositories.Tests.Acceptance.Drivers;

public class JsonDriver
{
    protected readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public T Map<T>(object data)
    {
        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(data, SerializerOptions), SerializerOptions);
    }
}