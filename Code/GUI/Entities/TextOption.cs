namespace GUI.Entities;

public class TextOption : TextComponent, IComparable<TextOption>
{
    public List<string> Options { get; set; } = [];

    public string LastSelected { get; set; } = string.Empty;
    
    public override int CompareTo(TextComponent? other)
    {
        if (other is TextOption textOption)
        {
            return CompareTo(textOption);
        }

        return base.CompareTo(other);
    }
    public int CompareTo(TextOption? other)
    {
        if (other == null) return 1;

        var countComparisionResult = Options.Count.CompareTo(other.Options.Count);

        if (countComparisionResult != 0) return 1;

        foreach (var option in Options)
        {
            var itemFound = false;
            foreach (var otherOption in other.Options)
            {
                var comparedOptionResult = String.Compare(option, otherOption, StringComparison.Ordinal);
                if (comparedOptionResult == 0) itemFound = true;
            }

            if (!itemFound) return 1;
        }

        return 0;
    }
}