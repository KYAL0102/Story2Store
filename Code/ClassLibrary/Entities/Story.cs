namespace ClassLibrary.Entities;

public class Story : IComparable<Story>, ICloneable
{
    public int Id { get; set; } = -1;
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

        for (var x = 0; x < Components.Count; x++)
        {
            var nativeComponent = Components[x];
            var remoteComponent = other.Components[x];

            var result = nativeComponent.CompareTo(remoteComponent);
            if (result != 0) return result;
        }

        return 0;
    }

    public object Clone()
    {
        return new Story
        {
            Title = (string) this.Title.Clone(),
            Components = this.Components
                            .Select(item => item.Clone())
                            .Cast<TextComponent>()
                            .ToList()
        };
    }
}