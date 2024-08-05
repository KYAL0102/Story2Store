namespace GUI.Entities;

public record Story : IComparable<Story>
{
    public string Title { get; set; } = string.Empty;

    public List<TextComponent> Components { get; init; } = [];

    public int CompareTo(Story? other)
    {
        if (other == null) return 1;
        
        var titleComparison = string.Compare(Title, other.Title, StringComparison.Ordinal);
        if (titleComparison != 0)
        {
            return titleComparison;
        }
        
        var componentCountComparison = Components.Count.CompareTo(other.Components.Count);
        if (componentCountComparison != 0)
        {
            return componentCountComparison;
        }

        foreach (var component in Components)
        {
            var componentFound = false;
            foreach (var otherComponent in other.Components)
            {
                var componentComparison = component.CompareTo(otherComponent);
                if (componentComparison == 0) componentFound = true;
            }

            if (!componentFound) return 1;
        }

        return 0;
    }
}