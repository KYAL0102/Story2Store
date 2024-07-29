namespace GUI.Entites;

public class TextGuid
{
    public Guid Guid { get; } = Guid.NewGuid();

    public TextComponent TextComponent { get; set; } = TextComponent.TextField;
}