namespace GUI.Entities;

public class TextField : TextComponent, IComparable<TextField>
{
    public string Content { get; set; } = string.Empty;

    public override int CompareTo(TextComponent? other)
    {
        if (other is TextField textField)
        {
            return CompareTo(textField);
        }

        return base.CompareTo(other);
    }

    public int CompareTo(TextField? other)
    {
        if (other == null) return 1;

        var result = String.Compare(Content, other.Content, StringComparison.Ordinal);
        Console.WriteLine(result);
        return result;
    }
}