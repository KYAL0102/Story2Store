namespace GUI.Entities;

public record StoryLight
{
    public int Id { get; init; } = -1;
    public string Title { get; init; } = string.Empty;
    public int ComponentCount { get; init; }
}