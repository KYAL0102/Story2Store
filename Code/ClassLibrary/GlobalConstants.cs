using System.Text.Json;
using ClassLibrary.Converter;

namespace ClassLibrary;

public static class GlobalConstants
{
    public static readonly JsonSerializerOptions JsonOption = new()
    {
        Converters = { new TextComponentConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
}