using System.Text.Json;
using ClassLibrary.Converter;

namespace ClassLibrary;

public static class GlobalConstants
{
    public static readonly int BackendPort = 5050;
    public static readonly int FrontendPort = 4200;
    public static readonly JsonSerializerOptions JsonOption = new()
    {
        Converters = { new TextComponentConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };
}