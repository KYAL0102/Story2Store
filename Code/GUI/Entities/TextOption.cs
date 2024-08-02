namespace GUI.Entities;

public class TextOption : TextComponent
{
    public List<string> Options { get; set; } = [];

    public string LastSelected { get; set; } = string.Empty;
}