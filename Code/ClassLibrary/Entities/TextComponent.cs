namespace ClassLibrary.Entities;

public class TextComponent : IComparable<TextComponent>
{
    public Guid Guid { get; } = Guid.NewGuid();
    public virtual int CompareTo(TextComponent? other)
    {
        if (other == null) return 1;

        return Guid.CompareTo(other.Guid);
    }
}