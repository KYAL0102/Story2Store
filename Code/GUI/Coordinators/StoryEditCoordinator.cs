using System.Reactive.Linq;
using System.Reactive.Subjects;
using GUI.Entites;

namespace GUI.Coordinators;

public class StoryEditCoordinator
{
    private readonly BehaviorSubject<Guid> _observableProperty = new(Guid.Empty);
    public IObservable<Guid> ObservableProperty => _observableProperty.AsObservable();
    public Guid SelectedOption
    {
        get => _observableProperty.Value;
        set => _observableProperty.OnNext(value);
    }

    public string StoryTitle { get; } = "Meeting";
    
    public List<TextComponent> Components { get; } = 
    [
        new TextComponent
        {
            Type = TextComponentType.TextField,
            Content = "Last"
        },
        new TextComponent
        {
            Type = TextComponentType.TextOption,
            Content = "night;evening;Christmas"
        },
        new TextComponent
        {
            Type = TextComponentType.TextField,
            Content = ", I met my old"
        },
        new TextComponent
        {
            Type = TextComponentType.TextOption,
            Content = "friend;grandpa;rival"
        },
        new TextComponent
        {
            Type = TextComponentType.TextField,
            Content = "and we talked a little bit."
        }
    ];
}