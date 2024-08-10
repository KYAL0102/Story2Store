using System.Text.Json;

namespace ClassLibrary;

public static class GlobalConstants
{
    public static readonly JsonSerializerOptions JsonOption = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
}