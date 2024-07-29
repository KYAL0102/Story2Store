namespace GUI.Entites;

public class TextComponent
{
    public Guid Guid { get; } = Guid.NewGuid();

    public TextComponentType Type { get; init; } = TextComponentType.TextField;

    public string Content { get; set; } = string.Empty;
}