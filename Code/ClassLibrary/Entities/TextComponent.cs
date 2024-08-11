using System.Text.Json.Serialization;
using ClassLibrary.Converter;

namespace ClassLibrary.Entities;

[JsonConverter(typeof(TextComponentConverter))]
public class TextComponent : IComparable<TextComponent>
{
    public Guid Guid { get; init; } = Guid.NewGuid();
    public virtual int CompareTo(TextComponent? other)
    {
        if (other == null) return 1;

        return Guid.CompareTo(other.Guid);
    }
}